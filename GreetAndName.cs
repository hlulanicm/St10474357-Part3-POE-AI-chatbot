using System;

namespace ai_response
{

    //  GreetAndName.cs:  Handles all greeting and farewell messages.
    //  MainWindow calls these methods so all user facing text lives here.
    //IN this file i updated Greet and Name file



    public class GreetAndName
    {
        private Random rand = new Random();



        public string GetName( string enteredName)
        {
            if (string.IsNullOrEmpty(enteredName))
                return "user ";
            //if the user does not entere any names the system must just save them as user


            return enteredName.Trim();

        }
        public string NewUserGreeting(string name)
        {
            string[] messages =
            {
                "=Hello, " + name + "! Its good to see you. Ask me anything about cybersecurity.",
                "Good to have you back, " + name + "! Ready to keep you safe online. What would you like to know?",
                "Hey " + name + ",Iam cyber bot a cyber security AI What cybersecurity topic can I help you with today?",
                "It is great to see you again, " + name + "! Ask away , I am here to help."
           
        };
            return messages[rand.Next(messages.Length)];
        }

        public string WelcomeBack(string name)
        {
            string[] messages =
            {
                "Welcome back, " + name + "! Great to see you again. Ask me anything about cybersecurity.",
                "Good to have you back, " + name + "! Ready to keep you safe online. What would you like to know?",
                "Hey " + name + ", welcome back! What cybersecurity topic can I help you with today?",
                "It is great to see you again, " + name + "! Ask away — I am here to help."

        };
            return messages[rand.Next(messages.Length)];
        }





public string Goodbye(string name)

        {
            string displayName = string.IsNullOrWhiteSpace(name) ? "friend" : name;

            string[] messages =
           {
                "Goodbye, " + displayName + "! Stay safe online!",
                "Take care, " + displayName + "! Remember to stay vigilant online.",
                "See you next time, " + displayName + "! Keep your passwords strong and your guard up.",
                "Farewell, " + displayName + "! Stay cyber-safe out there."
            };
            return messages[rand.Next(messages.Length)];


            }

        }
}
