using FoxTrader.Game;
using FoxTrader.Game.Utils;
using FoxTrader.UI;
using FoxTrader.UI.Control;
using FoxTrader.UI.Skin;
using static FoxTrader.Constants;

namespace FoxTrader.Views
{
    class TestView : GameView
    {
        private readonly Label m_label1;
        private readonly Label m_label2;
        private readonly Label m_label3;
        private readonly Label m_label4;
        private readonly Label m_label5;

        public TestView(GameControl c_controlParent) : base(c_controlParent)
        {
            Dock = Pos.Fill;

            var a_centerButton = new Button(this);
            a_centerButton.Text = "Gen Name";
            a_centerButton.SetSize(250, 40);
            a_centerButton.SetPosition(10, 10);
            a_centerButton.Clicked += button_Clicked;

            var a_centerButton2 = new Button(this);
            a_centerButton2.Text = "Gen Full Name";
            a_centerButton2.SetSize(250, 40);
            a_centerButton2.SetPosition(10, 60);
            a_centerButton2.Clicked += button_Clicked;

            var a_centerButton3 = new Button(this);
            a_centerButton3.Text = "Gen Vec2";
            a_centerButton3.SetSize(250, 40);
            a_centerButton3.SetPosition(10, 110);
            a_centerButton3.Clicked += button_Clicked;

            var a_centerButton4 = new Button(this);
            a_centerButton4.Text = "Gen Vec3";
            a_centerButton4.SetSize(250, 40);
            a_centerButton4.SetPosition(10, 160);
            a_centerButton4.Clicked += button_Clicked;

            m_label1 = new Label(this);
            m_label1.Text = "[ ]";
            m_label1.SetSize(640, 40);
            m_label1.SetPosition(270, 10);
            m_label1.MakeColorBright();
            m_label1.Alignment = Pos.Left | Pos.CenterV;

            m_label2 = new Label(this);
            m_label2.Text = "[ ]";
            m_label2.SetSize(640, 40);
            m_label2.SetPosition(270, 60);
            m_label2.MakeColorBright();
            m_label2.Alignment = Pos.Left | Pos.CenterV;

            m_label3 = new Label(this);
            m_label3.Text = "[ ]";
            m_label3.SetSize(640, 40);
            m_label3.SetPosition(270, 110);
            m_label3.MakeColorBright();
            m_label3.Alignment = Pos.Left | Pos.CenterV;

            m_label4 = new Label(this);
            m_label4.Text = "[ ]";
            m_label4.SetSize(640, 40);
            m_label4.SetPosition(270, 160);
            m_label4.MakeColorBright();
            m_label4.Alignment = Pos.Left | Pos.CenterV;

            m_label5 = new Label(this);
            m_label5.Text = "[ ]";
            m_label5.SetSize(920, 40);
            m_label5.SetPosition(10, 210);
            m_label5.MakeColorBright();
            m_label5.Alignment = Pos.Left | Pos.CenterV;
        }

        protected override void Render(SkinBase c_skin)
        {
            m_label5.Text = $"[ {Time.SpaceTime:r} ]";

            base.Render(c_skin);
        }

        private void button_Clicked(GameControl c_control)
        {
            var a_button = c_control as Button;

            if (a_button == null)
            {
                return;
            }

            switch (a_button.Text)
            {
                case "Gen Name":
                {
                    m_label1.Text = $"[ {Generator.Name()} ]";
                }
                break;

                case "Gen Full Name":
                {
                    m_label2.Text = $"[ {Generator.FullName()} ]";
                }
                break;

                case "Gen Vec2":
                {
                    var a_randomVec2 = Vector2.Random(kGalaxySizeMax);
                    m_label3.Text = $"[ Vec2({a_randomVec2.X}, {a_randomVec2.Y}) ]";
                }
                break;

                case "Gen Vec3":
                {
                    var a_randomVec3 = Vector3.Random(kGalaxySizeMax);
                    m_label4.Text = $"[ Vec3({a_randomVec3.X}, {a_randomVec3.Y}, {a_randomVec3.Z}) ]";
                }
                break;
            }
        }
    }
}
