using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;
using Xamarin.Essentials;
using Xamarin.Forms;
using System.Threading;

namespace DC_Motor
{
    public partial class MainPage : ContentPage
    {
        double sliderData;
        int read;
        public MainPage()
        {
            InitializeComponent();
            MessagingCenter.Subscribe<SliderLabelControl, double>(this, "slider", (sender, e) =>
            {
                sliderData = e;
                read = (int)Math.Round((decimal)sliderData);
                SliderResult.Text = read.ToString() + "%";
                MessagingCenter.Send<MainPage, int>(this, "dc_motor", read);
            });
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            MessagingCenter.Send<MainPage, int>(this, "dc_motor", 0); // OFF
        }

        private void Button_Clicked_1(object sender, EventArgs e)
        {
            MessagingCenter.Send<MainPage, int>(this, "dc_motor", 1); // Clockwise
        }

        private void Button_Clicked_2(object sender, EventArgs e)
        {
            MessagingCenter.Send<MainPage, int>(this, "dc_motor", 2); // Counterclockwise
        }
    }

    class SliderLabelControl : AbsoluteLayout
    {
        private Label _sliderValue;
        private Slider _slider;
        public SliderLabelControl()
        {
            AddControls();
        }

        private void AddControls()
        {
            _slider = new Slider { Margin = new Thickness(0, 20, 0, 0) };
            _slider.ValueChanged += (s, e) =>
            {
                PositionLabel(e.NewValue);
                double sliderData;
                sliderData = e.NewValue;

                MessagingCenter.Send(this, "slider", sliderData);
            };

            _sliderValue = new Label();
            _sliderValue.SetBinding(Label.TextProperty, new Binding("Value", source: _slider) { StringFormat = "{0:F0}" });

            SetLayoutFlags(_slider, AbsoluteLayoutFlags.All);
            SetLayoutBounds(_slider, new Rectangle(0f, 1f, 1f, 1f));

            Children.Add(_sliderValue);
            Children.Add(_slider);
        }

        private void PositionLabel(double newValue)
        {
            if (newValue == 0.0) return;

            var xPosition = (newValue - _slider.Minimum) / (_slider.Maximum - _slider.Minimum);

            SetLayoutFlags(_sliderValue, AbsoluteLayoutFlags.PositionProportional);
            SetLayoutBounds(_sliderValue, new Rectangle(xPosition, 0f, AutoSize, AutoSize));
        }

        public double Minimum
        {
            get => (double)_slider.GetValue(Slider.MinimumProperty);
            set => _slider.SetValue(Slider.MinimumProperty, value);
        }

        public double Maximum
        {
            get => (double)_slider.GetValue(Slider.MaximumProperty);
            set => _slider.SetValue(Slider.MaximumProperty, value);
        }
    }
}
