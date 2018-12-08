using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
namespace StanfordNLP
{
    class NLP
    {
        private string[] nonKeyWords = { " and"," to", " of", " with", " at", " from", " in", " into", " for", " on", " by", " like",
            " the", " an", " can", " shall", " will", " am", " is", " are", " were", " has", " have" , " could", " would", " should",
            " might", " may", " me", " he", " she", " you"};

        public string getProb = "";
        public int[][] ModelWordMatix;
        string[] wordVocabulary;
        string[][] wordSplitQuestioAnswer;
        string[][] wordToMatrix;
        public NLP()
        {
            wordSplitQuestioAnswer = SplitQuestionAnswer(ReadTestData());
            wordToMatrix = WordToMatrix(KeyWords(wordSplitQuestioAnswer));
            wordVocabulary = Vocabulary(KeyWords(SplitQuestionAnswer(ReadTestData())));
            ModelWordMatix = WordToNumberMatrix(wordToMatrix, wordVocabulary);

        }



        public string[] ReadTestData()
        {
            string path = Directory.GetCurrentDirectory();
            string[] lines = File.ReadAllLines(path + "\\training.txt");
            return lines;
        }

        public string[][] SplitQuestionAnswer(string[] readTestData)
        {

            string[] data = readTestData;
            string[][] qusAns = new string[data.Length][];
            for (int i = 0; i < data.Length; i++)
            {
                string[] splitstring = data[i].Split(new[] { "ans:" }, StringSplitOptions.None);
                qusAns[i] = new string[2];
                for (int ii = 0; ii < splitstring.Length; ii++)
                {
                    qusAns[i][ii] = splitstring[ii].ToString();
                }
                splitstring = null;
            }

            return qusAns;
        }

        public string[][] KeyWords(string[][] splitQuestioAnswer)
        {
            string[][] qusAns = splitQuestioAnswer;

            for (int i = 0; i < qusAns.Length; i++)
            {
                for (int ii = 0; ii < nonKeyWords.Length; ii++)
                {
                    qusAns[i][0] = qusAns[i][0].Replace(nonKeyWords[ii], "").Replace("  ", " ");
                }
            }

            return qusAns;
        }

        public string[] Vocabulary(string[][] keyWords)
        {
            string vocabularyline = "";

            string[][] keywords = keyWords;

            for (int i = 0; i < keyWords.Length; i++)
            {
                vocabularyline = vocabularyline + " " + keywords[i][0];
            }
            var items = vocabularyline.Split(' ');
            var unique_items = new HashSet<string>(items);
            string[] vocabulary = new string[unique_items.Count];
            int counter = 0;
            foreach (string s in unique_items)
            {
                if (s.Trim() != "")
                {
                    // Console.WriteLine(counter + ":" + s.Trim());
                    vocabulary[counter] = s.Trim();
                }

                // Console.WriteLine(counter.ToString());
                counter++;
            }

            return vocabulary;
        }

        public string[][] WordToMatrix(string[][] keyWords)
        {
            string[][] wordToMatrixArray = new string[keyWords.Length][];
            string[][] keyline = keyWords;
            for (int i = 0; i < wordToMatrixArray.Length; i++)
            {
                wordToMatrixArray[i] = new string[10];
                string[] words = keyline[i][0].Split(new[] { " " }, StringSplitOptions.None);
                for (int ii = 0; ii < words.Length; ii++)
                {
                    wordToMatrixArray[i][ii] = words[ii];
                }
            }
            return wordToMatrixArray;
        }


        public int[][] WordToNumberMatrix(string[][] wordToMatrix, string[] vocabulary)
        {
            int[][] wordToNumberMatrixArray = new int[wordToMatrix.Length][];
            string[][] wordmatrix = wordToMatrix;
            string[] vocab = vocabulary;

            for (int i = 0; i < wordToNumberMatrixArray.Length; i++)
            {
                wordToNumberMatrixArray[i] = new int[10];
                for (int ii = 0; ii < 10; ii++)
                {
                    for (int iii = 0; iii < vocab.Length; iii++)
                    {
                        if (wordmatrix[i][ii] == vocab[iii])
                        {
                            wordToNumberMatrixArray[i][ii] = iii;
                        }
                    }
                }
            }
            return wordToNumberMatrixArray;
        }

        public double[] NNProcess(int[] question)
        {
            getProb = "";
            double[] ansCode = new double[2];
            double[] matchingScore = new double[ModelWordMatix.Length];
            double[] probabilityScore = new double[ModelWordMatix.Length];
            double[] numberOfWords = new double[ModelWordMatix.Length];
            // double[] score = new double[foundRows.Length];
            for (int i = 0; i < ModelWordMatix.Length; i++)
            {
                for (int ii = 0; ii < 10; ii++)
                {
                    for (int iii = 0; iii < question.Length; iii++)
                    {
                        if (ModelWordMatix[i][ii] != 0)
                        {
                            if (question[iii] == ModelWordMatix[i][ii])
                            {
                                //Console.WriteLine(question[iii].ToString());
                                matchingScore[i]++;
                            }
                        }

                    }
                    if (ModelWordMatix[i][ii] != 0)
                    {
                        numberOfWords[i]++;
                    }
                }
                probabilityScore[i] = (matchingScore[i] / numberOfWords[i]);
                //  probabilityScore[i] = (matchingScore[i] / question.Length);
                //   Console.WriteLine(i + ": " + probabilityScore[i].ToString());
            }

            double maxscore = probabilityScore.Max();

            int scorePosition = Array.IndexOf(probabilityScore, maxscore);

            ansCode[0] = maxscore;
            ansCode[1] = scorePosition;
            getProb = ansCode[0].ToString();
            return ansCode;
        }

        public int[] QuestionToArray(string question)
        {

            string[] questionsplit = question.Split(new[] { " " }, StringSplitOptions.None);

            int[] questionToVocabNumber = new int[questionsplit.Length];


            for (int i = 0; i < questionsplit.Length; i++)
            {
                for (int ii = 0; ii < wordVocabulary.Length; ii++)
                {
                    if (questionsplit[i] == wordVocabulary[ii])
                        questionToVocabNumber[i] = ii;
                }

            }

            return questionToVocabNumber;
        }

        public string getAnswer(string question)
        {
            string ans = "";
            double[] resultNN = NNProcess(QuestionToArray(question));
            //Console.WriteLine("-------");
            //Console.WriteLine(resultNN[0].ToString("0.##"));
            // Console.WriteLine(resultNN[1].ToString());
            ans = wordSplitQuestioAnswer[Convert.ToInt16(resultNN[1])][1];
            return ans.Trim();
        }

        public string getProbability(string question)
        {
            double[] resultNN = NNProcess(QuestionToArray(question));
            return getProb;
        }
    }
}
