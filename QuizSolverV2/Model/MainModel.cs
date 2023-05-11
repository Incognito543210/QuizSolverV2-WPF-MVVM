﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SQLite;
using System.Collections.ObjectModel;

namespace QuizSolverV2.Model
{
    internal class MainModel
    {
        string connectionString = "Data Source=\"C:\\Users\\Pumpel\\Desktop\\QuizDataBase.db\"";
        public ObservableCollection<Quiz> quizList = new ObservableCollection<Quiz>(); 

       public void loadQuizList()
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM quizTable";

                using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(query, connection))
                {

                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    foreach (DataRow row in dataTable.Rows)
                    {
                        Quiz quiz = new Quiz();
                        quiz.id_quiz = Convert.ToInt32(row["id_quiz"]);
                        quiz.name = row["name"].ToString();
                        Console.WriteLine(quiz.id_quiz + quiz.name);
                        quizList.Add(quiz);
                    }

                }
                connection.Close();
            }
        }

        public void loadQuestionAndAnswer(int id_quizParametr = 1)
        {

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string queryQuestion = "SELECT * FROM questionTable where id_quiz =@param_id_quiz";
                

                using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(queryQuestion, connection))
                {
                    adapter.SelectCommand.Parameters.AddWithValue("@param_id_quiz", id_quizParametr.ToString()) ;
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    foreach (DataRow row in dataTable.Rows)
                    {
                        Questions question = new Questions();

                        question.id_question = Convert.ToInt32(row["id_question"]);
                        question.question = row["content"].ToString();
                        question.corret_answer = Convert.ToInt32(row["id_correctAnswer"]);
                        question.id_quiz = Convert.ToInt32(row["id_quiz"]);
                        Console.WriteLine(question.id_question + question.question + question.corret_answer + question.id_quiz);


                        quizList[question.id_quiz].questions.Add(question);

                    }

                }

                string queryAnswer = "SELECT * FROM answerTable where id_question =@param_id_question";

                for (int i = 0; i< quizList[id_quizParametr].questions.Count; i++)
                {



                    using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(queryAnswer, connection))
                    {
                        adapter.SelectCommand.Parameters.AddWithValue("@param_id_question", quizList[id_quizParametr].questions[i].id_question.ToString());
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        foreach (DataRow row in dataTable.Rows)
                        {
                            Answer anserws = new Answer();

                            anserws.content = row["contentAnswer"].ToString();
                            anserws.id_answer = Convert.ToInt32(row["id_answer"]);
                            anserws.id_question = Convert.ToInt32(row["id_question"]);
                            Console.WriteLine(anserws.content+ anserws.id_answer+ anserws.id_question);

                           quizList[id_quizParametr].questions[i].answers.Add(anserws);
                        }

                    }


                }


                connection.Close();
            }

        }



















    }
}
