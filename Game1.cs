using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace BrickOut
{
    public class Game1 : Game
    {
        private Dictionary<String, Texture2D> textures;
        private ParallaxBackground parallaxBackground;
        private Dictionary<String, SpriteFont> fonts;
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private List<Brick> bricks;
        private Paddle paddle;
        private Random rnd;
        private Ball ball;

        public static List<HoverText> hoverTexts;
        public static Rectangle screenBounds;
        public static int score;
        public static int lives;
        public static bool paddleHittingWall;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 500;
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            screenBounds = new Rectangle(0, 0, (int) graphics.PreferredBackBufferWidth, (int) graphics.PreferredBackBufferHeight);
            this.Window.Title = "BrickOut";

            hoverTexts = new List<HoverText>();

            rnd = new Random();

            score = 0;
            lives = 5;

            paddleHittingWall = false;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            parallaxBackground = new ParallaxBackground(
                Content.Load<Texture2D>("Background\\landscape_1"),
                Content.Load<Texture2D>("Background\\landscape_2"),
                Content.Load<Texture2D>("Background\\landscape_3"),
                Content.Load<Texture2D>("Background\\landscape_4"),
                Content.Load<Texture2D>("Background\\landscape_5"),
                Content.Load<Texture2D>("Background\\landscape_6")
            );

            fonts = new Dictionary<string, SpriteFont>();
            fonts.Add("BrickFont", Content.Load<SpriteFont>("Fonts\\BrickFont"));
            fonts.Add("LivesFont", Content.Load<SpriteFont>("Fonts\\LivesFont"));

            textures = new Dictionary<string, Texture2D>();
            textures.Add("Brick", Content.Load<Texture2D>("Textures\\Brick"));
            textures.Add("Ball", Content.Load<Texture2D>("Textures\\Ball"));
            textures.Add("Paddle", Content.Load<Texture2D>("Textures\\Paddle"));
            textures.Add("BrickPoweredGlint", Content.Load<Texture2D>("Animations\\PoweredSpriteSheet"));

            paddle = new Paddle(textures["Paddle"], 100, 20);
            ball = new Ball(textures["Ball"], 16, 16, paddle.getBounds().X, paddle.getBounds().Y, Ball.MovementState.FOLLOW_PADDLE, fonts["BrickFont"]);

            int rows = 5;
            int columns = 20;
            bricks = new List<Brick>();
            for (int row = 0; row < rows; row++)
            {
                for (int column = 0; column < columns; column++)
                {
                    int hitPoints = 104 - (int) Math.Pow((row + 1) * 2, 2);
                    int width = (int) (screenBounds.Width / columns);

                    Brick.BrickType brickType = Brick.BrickType.NORMAL;
                    int brickTypeDecision = rnd.Next(0, 21);
                    if (brickTypeDecision == 20)
                    {
                        brickType = Brick.BrickType.POWERED;
                        hitPoints = 1;
                    }

                    bricks.Add(
                        new Brick(textures["Brick"], 
                        fonts["BrickFont"], 
                        width, 
                        width, 
                        width * column, 
                        width * row, 
                        hitPoints, 
                        brickType, 
                        new AnimationSheet(textures["BrickPoweredGlint"], 100, 100, 100, 1000, spriteBatch.GraphicsDevice)));
                }
            }
        }

        public void triggerGameEnd()
        {
            Exit();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            parallaxBackground.update();

            for (int i = bricks.Count - 1; i >= 0; i--)
            {
                bricks[i].update(gameTime);
                if (bricks[i].getHitPoints() <= 0) bricks.Remove(bricks[i]);
            }

            paddle.update();
            ball.update(paddle.getBounds(), bricks);

            for (int i = hoverTexts.Count - 1; i >= 0; i--)
                if (hoverTexts[i].getAlpha() <= 0.0) hoverTexts.Remove(hoverTexts[i]);
            foreach (HoverText hoverText in hoverTexts)
                hoverText.update();

            if ((Keyboard.GetState().IsKeyDown(Keys.W) || Keyboard.GetState().IsKeyDown(Keys.Up)) && ball.getMovementState() != Ball.MovementState.BOUNCE) 
            { 
                ball.setMovementstate(Ball.MovementState.BOUNCE);

                int dir = rnd.Next(0, 2);
                if (dir == 0) dir = -1;
                else dir = 1;

                ball.setVelocity((float) (rnd.Next(3, 8) * dir), (float) (rnd.Next(3, 8) * dir));
            }

            if (lives == 0) triggerGameEnd();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();

            parallaxBackground.draw(spriteBatch);

            foreach (Brick brick in bricks)
            {
                brick.draw(spriteBatch);
            }

            foreach (HoverText hoverText in hoverTexts)
                hoverText.draw(spriteBatch);

            spriteBatch.DrawString(fonts["LivesFont"], "Lives: " + lives, new Vector2(10, screenBounds.Height - 30), Color.Red);

            paddle.draw(spriteBatch);
            ball.draw(spriteBatch);

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}