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
            answerButtons = new List<Button>();

            LoadQuestions();//calling the questions method and displaying all the questions
            ShuffleQuestions();//mix questions areound for senitent user interactions
            ShowQuestion();//display all the questions




            
            //Listing all the buttons we will be using in this instance 
        }


        private void LoadQuestion()

        {
            questions = new List<QuizQuestion>
            {

                new QuizQuestion(
                    "What does 2FA stand for?",//displayed question
                    new[] { "Two-Factor Authentication", "Two-File Access", "Trusted Firewall Application", "Two-Frequency Algorithm" },
                    0,
               "Correct , two factor authentication stands for that "






        }

        //getters for retrieving and loading all the quiz questions 
        public class QuizQuestion
        {
            public string Question { get; }
            public string[] Options { get; }
            public int CorrectIndex { get; }
            public string Feedback { get; }

            public QuizQuestion(string question, string[] options, int correctIndex, string feedback)
            {
                Question = question;
                Options = options;
                CorrectIndex = correctIndex;
                Feedback = feedback;
            }
        }


    }
}