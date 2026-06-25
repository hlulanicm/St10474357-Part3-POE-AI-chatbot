using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;


namespace ai_response
{
    public class InterestTracker
    {
        private readonly string filename = "interested_topic.txt"; // The file where all user interests are saved
        public string SaveInterests(string username, string[] words, ArrayList ignore)
        {
            // create a HashSet to store the interests found in the users input while also avoiding duplicates
            HashSet<string> currentInterests = new HashSet<string>();
            foreach (string interest in words)
            {
                //clean each user input by turning it into lower case and rejecting special characters
                string clean = Regex.Replace(interest.ToLower().Trim(), @"[^a-zA-Z0-9\s]", "");

                // Only add the word if it is not in the ignore list and if it is long enough
                if
                    (!ignore.Contains(clean) &&
                    clean != "interested" &&
                    clean != "and" &&
                    clean != "in" &&

                    clean.Length >= 3)
                {
                    currentInterests.Add(clean); //add the word
                }
            }

            // If no valid interests were found after filtering, prompt the user to be more specific
            if (currentInterests.Count == 0)
                return "Please specify what you are interested in (e.g. 'I am interested in phishing').";

            // Join all found interests into a single comma separated string for storage and display
            string store = string.Join(", ", currentInterests);



            bool userFound = false; 
            // Tracks whether the username already exists in the file

               if (File.Exists(filename))
                {
                // Read all existing lines from the interests file
                   string[] lines = File.ReadAllLines(filename);



                for (int i = 0; i < lines.Length; i++)
                {
                    // Check if this line belongs to the current user
                    if (lines[i].StartsWith(username))
                    {
                        userFound = true;

                        // Strip the username and the interest so we can identify the interest
                        string existing = lines[i]
                            .Replace(username + " interested in:", "")
                            .ToLower();


                        //Load handling 
                        // Load the existing interests into a HashSet so we can merge without duplicates
                        HashSet<string> existingSet = new HashSet<string>(

                            existing.Split(',')
                                    .Select(x => x.Trim())
                                    .Where(x => x.Length > 0));

                     //adding the user interests into existing set
                        foreach (string item in currentInterests)


                            existingSet.Add(item);

                        // Overwrite the users line in the file with the updated merged interests
                        lines[i] = username + " interested in: " +
                                   string.Join(", ", existingSet);
                            File.WriteAllLines(filename, lines);




                        return "Great, I added " + store + " to your interests!"; // Confirm to the user what was added with an appropriate message
                    }
                }
            }


            if (!userFound)
            {
                File.AppendAllText(filename,
                    username + " interested in: " + store + "\n");
                return "Great, I will remember that you are interested in " + store + "!";//if the user is not found keep the interests if they are not new then we will update interests 
            }

            return string.Empty;
        }

        public string GetInterests(string username)
        {
            if (!File.Exists(filename)) return null;

            foreach (string line in File.ReadAllLines(filename))
            {
                if (!line.StartsWith(username)) continue;

                int colonIndex = line.IndexOf("interested in:");
                if (colonIndex < 0) return null;

                return line.Substring(colonIndex + 14).Trim();
            }

            return null;
        }
    }
}