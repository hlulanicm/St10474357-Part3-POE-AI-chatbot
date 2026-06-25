using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Chatbot
{
    public partial class quiz: Window 


        // Quiz logic styles and indexing 
    {


        private int currentIndex = 0; //the current score and quiz being displayed is none

        private int score = 0;//users score is null initially

        private List<int> Question > questions;

            Private List<Button> answerButtons;


        public quiz()
        {
            InitializeComponent();
            answeButtons = new List<Button>();  //Listing all the buttons we will be using in this instance 
        }




    }
}