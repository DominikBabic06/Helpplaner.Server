using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Helpplaner.Client.GUI.Behaviours
{
    public class ResponsivenessBehavior
    {

        // Responsive Property damit das Gui responsive wird hoffentlich lol
        public static readonly DependencyProperty IsResponsiveProperty =
        DependencyProperty.RegisterAttached("IsResponsive", typeof(bool), typeof(ResponsivenessBehavior),
            new PropertyMetadata(false));

        public static int GetIsResponsive(DependencyObject obj)
        {
            return (int)obj.GetValue(IsResponsiveProperty);
        }

        public static void SetIsResponsive(DependencyObject obj, int value)
        {
            obj.SetValue(IsResponsiveProperty, value);
        }




        // Machen einen breakpoint für horizontal alignment damit es sich ab da responsicve verhält und ändert
        public static readonly DependencyProperty HorizontalBreakpointProperty =
            DependencyProperty.RegisterAttached("HorizontalBreakpoint", typeof(double), typeof(ResponsivenessBehavior),
                new PropertyMetadata(double.MaxValue));

        public static int GetHorizontalBreakpoint(DependencyObject obj)
        {
            return (int)obj.GetValue(HorizontalBreakpointProperty);
        }

        public static void SetHorizontalBreakpoint(DependencyObject obj, int value)
        {
            obj.SetValue(HorizontalBreakpointProperty, value);
        }



        // Was passiert wenn der Bildschirm breiter als der Breakpoint oben ist? Das wird mit diesen Settern geregelt inshallah
        public static readonly DependencyProperty HorizontalBreakpointSettersProperty =
        DependencyProperty.RegisterAttached("HorizontalBreakpointSetters", typeof(SetterBaseCollection), typeof(ResponsivenessBehavior),
            new PropertyMetadata(new SetterBaseCollection()));

        public static int GetHorizontalBreakpointSetters(DependencyObject obj)
        {
            return (int)obj.GetValue(HorizontalBreakpointSettersProperty);
        }

        public static void SetHorizontalBreakpointSetters(DependencyObject obj, int value)
        {
            obj.SetValue(HorizontalBreakpointSettersProperty, value);
        }




        // Tracken ob diese Setter aktiv sind oder nicht
        public static readonly DependencyProperty IsHorizontalBreakpointSettersActiveProperty =
        DependencyProperty.RegisterAttached("IsHorizontalBreakpointSettersActive", typeof(bool), typeof(ResponsivenessBehavior),
            new PropertyMetadata(false));


        public static int GetIsHorizontalBreakpointSettersActive(DependencyObject obj)
        {
            return (int)obj.GetValue(IsHorizontalBreakpointSettersActiveProperty);
        }

        public static void SetIsHorizontalBreakpointSettersActive(DependencyObject obj, int value)
        {
            obj.SetValue(IsHorizontalBreakpointSettersActiveProperty, value);
        }

 


    }
}
