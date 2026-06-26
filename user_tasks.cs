using Microsoft.Data.SqlClient; // Install-Package Microsoft.Data.SqlClient

namespace Chatbot
{
    // Represents a single task row from the database
    public class TaskItem
    {
        public int TaskId { get; set; }
        public string TaskName { get; set; }
        public string Description { get; set; }
        public string TaskDate { get; set; }
        public string TaskStatus { get; set; }
    }

    public class user_tasks
    {
        // ── Connection strings ────────────────────────────────────────────────
        private string masterConn =
            @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True;";

        private string taskConn =
            @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=tasks_DB;Integrated Security=True;";


        //   Call from MainWindow constructor
        // Creates the database and table if they do not already exist

        public void initialize_database()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(masterConn))
                {
                    conn.Open();

                    // 1. Create DB if missing
                    string createDB =
                        "IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'tasks_DB') " +
                        "CREATE DATABASE tasks_DB;";
                    new SqlCommand(createDB, conn).ExecuteNonQuery();
                }

                // 2. Create table if missing (connect to the new DB)
                using (SqlConnection conn = new SqlConnection(taskConn))
                {
                    conn.Open();
                    string createTable = @"
                        IF NOT EXISTS (
                            SELECT * FROM sysobjects WHERE name='all_tasks' AND xtype='U')
                        CREATE TABLE all_tasks (
                            task_id          INT PRIMARY KEY IDENTITY(1,1),
                            task_name        VARCHAR(50)  NOT NULL,
                            description_task VARCHAR(100),
                            task_date        VARCHAR(20),
                            task_status      VARCHAR(20)  DEFAULT 'Pending'
                        );";
                    new SqlCommand(createTable, conn).ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(
                    "DB setup error: " + ex.Message, "Database Error");
            }
        }
//create a new task and allow the user to input the details of the task 
        public string add_task(string name, string description, string date)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(taskConn))
                {
                    conn.Open();
                    string sql =
                        "INSERT INTO all_tasks (task_name, description_task, task_date, task_status) " +
                        "VALUES (@name, @desc, @date, 'Pending');";

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@desc", description);
                    cmd.Parameters.AddWithValue("@date", date);
                    cmd.ExecuteNonQuery();
                }
                return $"Task '{name}' has been added successfully with a reminder for {date}.";
            }
            catch (Exception ex)
            {
                return "Error adding task: " + ex.Message;
            }
        }


        // READ  –  Get all tasks

        public List<TaskItem> get_all_tasks()
        {
            List<TaskItem> tasks = new List<TaskItem>();
            try
            {
                using (SqlConnection conn = new SqlConnection(taskConn))
                {
                    conn.Open();
                    string sql = "SELECT task_id, task_name, description_task, task_date, task_status FROM all_tasks;";
                    SqlDataReader reader = new SqlCommand(sql, conn).ExecuteReader();
                    while (reader.Read())
                    {
                        tasks.Add(new TaskItem
                        {
                            TaskId     = reader.GetInt32(0),

                            TaskName   = reader.GetString(1),

                            Description = reader.IsDBNull(2) ? "" : reader.GetString(2),
                            TaskDate   = reader.IsDBNull(3) ? "" : reader.GetString(3),
                            TaskStatus = reader.IsDBNull(4) ? "Pending" : reader.GetString(4)
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Error loading tasks: " + ex.Message);
            }
            return tasks;
        }

        // ─────────────────────────────────────────────────────────────────────
        // UPDATE  –  Mark a task as completed
        // ─────────────────────────────────────────────────────────────────────
        public string complete_task(int taskId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(taskConn))
                {
                    conn.Open();
              
                    string sql = "UPDATE all_tasks SET task_status = 'Completed' WHERE task_id = @id;";//Update the task in the database when the task is complete
                  
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@id", taskId);
                    cmd.ExecuteNonQuery();
                }
                return "Task marked as completed.";//Display and appropriate message 
            }
            catch (Exception ex)
            {
                return "Error completing task: " + ex.Message; //In the event of a mishap display an appropriate message 
            }
        }

   
        public string delete_task(int taskId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(taskConn))
                {
                    conn.Open();
                  
                    string sql = "DELETE FROM all_tasks WHERE task_id = @id;";//SQL deletion from database pending user input
                 
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@id", taskId);
                    cmd.ExecuteNonQuery();
                }
                return "Task deleted successfully.";
            }
            catch (Exception ex)
            {
                return "Error deleting task: " + ex.Message;
            }
        }


        /// <summary>
        /// An NLP (Natural Language Processing) simulation can mean a few different things depending on the context—ranging from how AI models are trained to how businesses test conversational systems.

        /// </summary>
       ///


        // Returns a human-readable response string.

        public string nlp_task_command(string input)
        {
            string lower = input.ToLower().Trim();

            // ── ADD / REMIND ─────────────────────────────────────────────────
            if (ContainsAny(lower, new[] {
                    "add task", "create task", "new task",
                    "remind me", "set reminder", "add reminder",
                    "i need to", "i want to", "schedule task" }))
            {
                // Try to pull out a date token (simple heuristic)
                string date = ExtractDate(lower);

                // Strip command keywords to get the task name
                string taskName = StripCommandWords(lower, new[] {
                    "add task", "create task", "new task", "remind me to",
                    "remind me", "set reminder", "add reminder",
                    "i need to", "i want to", "schedule task", "task" });

                taskName = Capitalise(taskName.Trim());
                if (taskName.Length < 2) taskName = "Cybersecurity Task";

                string description = "Added via chatbot NLP";
                if (date == "") date = DateTime.Now.AddDays(7).ToString("yyyy-MM-dd");

                return add_task(taskName, description, date);
            }

            //  VIEW task
            if (ContainsAny(lower, new[] {
                    "view task", "show task", "list task", "my task",
                    "see task", "display task", "what task", "all task",
                    "show my task", "get task" }))
            {
                List<TaskItem> tasks = get_all_tasks();
                if (tasks.Count == 0)
                    return "You have no tasks saved yet. Try saying 'Add task - Enable two-factor authentication'.";

                string result = $"You have {tasks.Count} task(s):\n";
                foreach (var t in tasks)
                    result += $"\n[{t.TaskId}] {t.TaskName} | {t.TaskStatus} | Due: {t.TaskDate}";
                return result;
            }

            // COMPLETE task
            if (ContainsAny(lower, new[] {
                    "complete task", "mark task", "finished task",
                    "done task", "task done", "completed task",
                    "mark complete", "i completed", "i finished" }))
            {
                int id = ExtractNumber(lower);
                if (id == -1)
                    return "Please specify the task number. Example: 'Complete task 2'";
                return complete_task(id);
            }

            // ── DELETE ───────────────────────────────────────────────────────
            if (ContainsAny(lower, new[] {
                    "delete task", "remove task", "cancel task",
                    "erase task", "drop task" }))
            {
                int id = ExtractNumber(lower);
                if (id == -1)
                    return "Please specify the task number. Example: 'Delete task 3'";
                return delete_task(id);
            }

            return null; 
        }


        private bool ContainsAny(string input, string[] keywords)
        {
            foreach (string kw in keywords)
                if (input.Contains(kw)) return true;
            return false;
        }

        private string StripCommandWords(string input, string[] words)
        {
            foreach (string w in words)
                input = input.Replace(w, "");
            return input.Trim();
        }

        private string Capitalise(string s)
        {
            if (string.IsNullOrEmpty(s)) return s;
            return char.ToUpper(s[0]) + s.Substring(1);
        }

        private int ExtractNumber(string input)
        {
            foreach (string token in input.Split(' '))
                if (int.TryParse(token, out int n)) return n;
            return -1;
        }

        private string ExtractDate(string input)
        {
            // Look for patterns like "in 7 days", "tomorrow", specific dates
            if (input.Contains("tomorrow"))
                return DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
            if (input.Contains("next week"))
                return DateTime.Now.AddDays(7).ToString("yyyy-MM-dd");
            if (input.Contains("today"))
                return DateTime.Now.ToString("yyyy-MM-dd");

            // "in X days"
            var match = System.Text.RegularExpressions.Regex.Match(input, @"in (\d+) day");
            if (match.Success && int.TryParse(match.Groups[1].Value, out int days))
                return DateTime.Now.AddDays(days).ToString("yyyy-MM-dd");

            return "";
        }

        //Connection test for seamless error handling 
        public void test_connection()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(taskConn))
                    conn.Open();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Connection failed: " + ex.Message);
            }
        }
    }
}
