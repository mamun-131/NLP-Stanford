using edu.stanford.nlp.pipeline;
using java.io;
using System.Collections.Generic;
using System.Web.Http;
using System.IO;
using java.util;
using Console = System.Console;
using System.Text.RegularExpressions;

namespace StanfordNLP
{
    public class ValuesController : ApiController
    {
        // GET api/values 
        public object Get()
        {
            //  return new string[] { "value1", "value2" };
            //  var apidata = new List<string>();
            return new { Foo= 100 };
        }

        // GET api/values/5 
        public object Get(string id)
        {
            var apidata = StanfordNLP(id);
            return apidata;
            //  return "mamun";
        }

        // POST api/values 
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5 
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5 
        public void Delete(int id)
        {
        }

        private object StanfordNLP(string input)
        {
            string npath = Directory.GetCurrentDirectory();

            string NLPquery = "";
            string getNoun = "";
            bool compound = false;
            var text = input;
            // Annotation pipeline configuration
            var props = new Properties();

            props.setProperty("annotators", "tokenize, ssplit, pos, lemma, ner, parse, dcoref");
            props.setProperty("pos.model", npath + @"\edu\stanford\nlp\models\pos-tagger\english-bidirectional\english-bidirectional-distsim.tagger");
            props.setProperty("ner.model", npath + @"\edu\stanford\nlp\models\ner\english.all.3class.distsim.crf.ser.gz");
            props.setProperty("parse.model", npath + @"\edu\stanford\nlp\models\lexparser\englishPCFG.ser.gz");
            props.setProperty("dcoref.demonym", npath + @"\edu\stanford\nlp\models\dcoref\demonyms.txt");
            props.setProperty("dcoref.states", npath + @"\edu\stanford\nlp\models\dcoref\state-abbreviations.txt");
            props.setProperty("dcoref.animate", npath + @"\edu\stanford\nlp\models\dcoref\animate.unigrams.txt");
            props.setProperty("dcoref.inanimate", npath + @"\edu\stanford\nlp\models\dcoref\inanimate.unigrams.txt");
            props.setProperty("dcoref.male", npath + @"\edu\stanford\nlp\models\dcoref\male.unigrams.txt");
            props.setProperty("dcoref.neutral", npath + @"\edu\stanford\nlp\models\dcoref\neutral.unigrams.txt");
            props.setProperty("dcoref.female", npath + @"\edu\stanford\nlp\models\dcoref\female.unigrams.txt");
            props.setProperty("dcoref.plural", npath + @"\edu\stanford\nlp\models\dcoref\plural.unigrams.txt");
            props.setProperty("dcoref.singular", npath + @"\edu\stanford\nlp\models\dcoref\singular.unigrams.txt");
            props.setProperty("dcoref.countries", npath + @"\edu\stanford\nlp\models\dcoref\countries");
            props.setProperty("dcoref.extra.gender", npath + @"\edu\stanford\nlp\models\dcoref\namegender.combine.txt");
            props.setProperty("dcoref.states.provinces", npath + @"\edu\stanford\nlp\models\dcoref\statesandprovinces");
            props.setProperty("dcoref.singleton.predictor", npath + @"\edu\stanford\nlp\models\dcoref\singleton.predictor.ser");
            props.setProperty("dcoref.big.gender.number", npath + @"\edu\stanford\nlp\models\dcoref\gender.map.ser.gz");
            props.setProperty("sutime.rules", npath + @"\edu\stanford\nlp\models\sutime\defs.sutime.txt, " + npath + @"\edu\stanford\nlp\models\sutime\english.holidays.sutime.txt, " + npath + @"\edu\stanford\nlp\models\sutime\english.sutime.txt");
            props.setProperty("sutime.binders", "0");
            props.setProperty("ner.useSUTime", "0");
            var pipeline = new StanfordCoreNLP(props);

            // Annotation
            var annotation = new Annotation(text);
            pipeline.annotate(annotation);

            List<string> NLPDATA = new List<string>();

            using (var stream = new ByteArrayOutputStream())
            {
                //  pipeline.prettyPrint(annotation, new PrintWriter(stream));
                pipeline.conllPrint(annotation, new PrintWriter(stream));

                string output = stream.toString();
                Console.WriteLine(output);

                string[] lines = Regex.Split(output, "[\r\n]+");
                // Console.WriteLine(lines.Length);
                string[][] wordMatrix = new string[lines.Length][];
                for (var i = 0; i < wordMatrix.Length; i++)
                {
                    wordMatrix[i] = new string[10];
                    string[] words = Regex.Split(lines[i], "[^a-zA-Z0-9]+");
                    // Console.WriteLine(words.Length);
                    for (int ii = 0; ii < words.Length; ii++)
                    {
                        wordMatrix[i][ii] = words[ii];
                    }
                }
                
                for (int i = 0; i < lines.Length; i++)
                {
                    for (int ii = 0; ii < wordMatrix[i].Length; ii++)
                    {
                        if (wordMatrix[i][ii] == "VB" || wordMatrix[i][ii] == "RP" || wordMatrix[i][ii] == "NN" || wordMatrix[i][ii] == "NNP")
                        {
                            NLPDATA.Add(wordMatrix[i][ii] + " " + wordMatrix[i][ii - 1] + " " + wordMatrix[i][6]);
                            NLPquery = NLPquery + " " + wordMatrix[i][ii - 1];
                        }
                        if (wordMatrix[i][ii] == "NN" || wordMatrix[i][ii] == "NNP" || wordMatrix[i][ii] == "NNS")
                        {
                            if (wordMatrix[i][6] == "compound" || wordMatrix[i][6] == "xcomp")
                            {
                                getNoun = wordMatrix[i][ii - 1];
                                compound = true;
                            }
                            if (wordMatrix[i][6] == "dep" && compound != true && wordMatrix[i][ii] == "NN")
                            {
                                getNoun = wordMatrix[i][ii - 1];
                            }
                            if (wordMatrix[i][6] == "dobj" && compound != true && wordMatrix[i][ii] == "NN")
                            {
                                getNoun = wordMatrix[i][ii - 1];
                            }
                        }
                    }
                }
                stream.close();
            }
            //Intent, Name, 
            NLP nLP = new NLP();
            // nLP.getAnswer(NLPquery.Trim());
            // Console.WriteLine(nLP.getProbability(getNoun.Trim()).ToString());
            string action = nLP.getAnswer(NLPquery.Trim());
            string nprob = nLP.getProbability(getNoun.Trim()).ToString();
            string aprob = nLP.getProbability(NLPquery.Trim()).ToString();
            if (action == getNoun)
            {
                nprob = "0";
                aprob = "0";
            }
            compound = false;
            return new { intent = action, noun = getNoun, action_prob = aprob, noun_prob = nprob };
        }
    }
}
