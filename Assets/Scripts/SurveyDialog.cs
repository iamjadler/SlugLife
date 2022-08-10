using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SurveyDialog : MonoBehaviour
{
    public PieChart pieChart;
    public TextMeshProUGUI surveyText;
    private class QAndA
    {
        public string question;
        public List<string> answers;
        public List<string> labels;
        public List<int> results;

        public QAndA(string q, List<string> a, List<string> newLabels, List<int> newResults)
        {
            question = q; answers = a; labels = newLabels; results = newResults;
        }
    }

    private List<QAndA> miqaDatabase = new();

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        InitializeDatabase();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void InitializeDatabase()
    {
        /* 0 */ miqaDatabase.Add(new("Does music affect your mood?", new() { "Yes", "No" }, new() { "Yes", "No" }, new() { 28, 0 }));
        /* 1 */ miqaDatabase.Add(new("Do you think the concept of gender is helpful to society?", new() { "Yes", "No" }, new() { "Yes", "No" }, new() { 15, 13 }));
        /* 2 */ miqaDatabase.Add(new("Are more important things easier or harder to remember than less important things?", new() { "Easier", "Harder" }, new() { "Easier", "Harder" }, new() { 26, 2 }));
        /* 3 */ miqaDatabase.Add(new("How often do you think witnesses remember things wrong?", new() { "Rarely", "Sometimes", "Often", "Usually" }, new() { "Rarely", "Sometimes", "Often", "Usually" }, new() { 0, 12, 1, 15 }));
        /* 4 */ miqaDatabase.Add(new("Would you be more willing to try a candy made from raw slug if a friend tried it first?", new() { "Yes", "No" }, new() { "Yes", "No" }, new() { 14, 14 }));
        /* 5 */ miqaDatabase.Add(new("Do you think silent films(no sound) or podcasts(no video) are more emotionally powerful?", new() { "Silent film", "Podcast" }, new() { "Film", "Podcast" }, new() { 18, 10 }));
        /* 6 */ miqaDatabase.Add(new("Is saying a message using music more powerful than saying it normally?", new() { "Yes", "No" }, new() { "Yes", "No" }, new() { 19, 9 }));
        /* 7 */ miqaDatabase.Add(new("Would the lack of a neutral option in a questionnaire upset you?", new() { "Yes", "No" }, new() { "Yes", "No" }, new() { 13, 15 }));
        /* 8 */ miqaDatabase.Add(new("Did you notice the subliminal message in the previous question?", new() { "Yes", "No" }, new() { "Yes", "No" }, new() { 17, 11 }));
        /* 9 */ miqaDatabase.Add(new("Do you display social cues (such as eye contact or facial expressions) consciously or unconsciously?", new() { "Consciously", "Unconsciously" }, new() { "Consciously", "Unconsciously" }, new() { 14, 14 }));
        /* 10 */ miqaDatabase.Add(new("If you’re working on a project and all your teammates give up, would you also give up?", new() { "Yes", "No" }, new() { "Yes", "No" }, new() { 4, 24 }));
        /* 11 */ miqaDatabase.Add(new("“Children should be rewarded with candy for their efforts.” vs “i think itd be awesome if kids get candy when they do good stuff” Which quote would make the speaker sound more trustworthy?", new() { "Quote 1", "Quote 2" }, new() { "Quote 1", "Quote 2" }, new() { 19, 9 }));
        /* 12 */ miqaDatabase.Add(new("Do you think you could be friends with someone who thinks differently than you?", new() { "Yes", "No" }, new() { "Yes", "No" }, new() { 25, 3 }));
        /* 13 */ miqaDatabase.Add(new("Do you notice social cues (such as eye contact or facial expressions) consciously or unconsciously?", new() { "Consciously", "Unconsciously" }, new() { "Consciously", "Unconsciously" }, new() { 17, 11 }));
        /* 14 */ miqaDatabase.Add(new("Would you be willing to try a candy made from raw slugs if it was popular?", new() { "Yes", "No" }, new() { "Yes", "No" }, new() { 8, 20 }));
        /* 15 */ miqaDatabase.Add(new("If someone said “Are you ok with this?” would you interpret it as getting a second opinion or asking for permission?", new() { "Opinion", "Permission" }, new() { "Opinion", "Permission" }, new() { 6, 22 }));
        /* 16 */ miqaDatabase.Add(new("Do you ever fear getting canceled?", new() { "Yes", "No" }, new() { "Yes", "No" }, new() { 9, 19 }));
        /* 17 */ miqaDatabase.Add(new("If a flat-earther changes their mind and decides that the Earth is round because it used to be a cube but the corners broke off, would you keep correcting them?", new() { "Yes", "No" }, new() { "Yes", "No" }, new() { 13, 15 }));
        /* 18 */ miqaDatabase.Add(new("Did you notice the the typo in the previous question? (No peeking >:( )", new() { "Yes", "No" }, new() { "Yes", "No" }, new() { 1, 27 }));
        /* 19 */ miqaDatabase.Add(new("If someone tries to convince you that slugs are faster than humans, would you get upset?", new() { "Yes", "No" }, new() { "Yes", "No" }, new() { 13, 15 }));
        /* 20 */ miqaDatabase.Add(new("“A Roman walks into a bar, holds up two fingers, and says ‘Five beers please’” vs “What’s red and bad for your teeth? A brick” Which is funnier?", new() { "Joke 1", "Joke 2" }, new() { "Joke 1", "Joke 2" }, new() { 13, 15 }));
        /* 21 */ miqaDatabase.Add(new("“We should spend less money on making candy” vs “But then we wouldn’t have any more candy” Which person would you agree with?", new() { "Speaker of quote 1", "Speaker of quote 2" }, new() { "Speaker 1", "Speaker 2" }, new() { 20, 8 }));
        /* 22 */ miqaDatabase.Add(new("Do you ever avoid things that you would do/say otherwise out of fear of getting canceled?", new() { "Yes", "No" }, new() { "Yes", "No" }, new() { 10, 18 }));
        /* 23 */ miqaDatabase.Add(new("Do you form emotional attachments to inanimate objects?", new() { "Yes", "No" }, new() { "Yes", "No" }, new() { 25, 3 }));
        /* 24 */ miqaDatabase.Add(new("Would you change your behavior if someone else thinks you’re a bad person even if you’re doing what you think is good?", new() { "Yes", "No" }, new() { "Yes", "No" }, new() { 9, 18 }));
    }

    public void Activate()
    {
        transform.Find("Canvas").gameObject.SetActive(true);
    }

    public void DeActivate()
    {
        transform.Find("Canvas").gameObject.SetActive(false);
    }

    public void SetQAndA(int index, string commentary="")
    {
        int cumsum = 0;
        QAndA qAndA = miqaDatabase[index];
        string text = qAndA.question;
        foreach (int result in qAndA.results)
        {
            cumsum += result;
        }
        for (int i=0; i<qAndA.answers.Count; i++)
        {
            text += "\n  "+ (i+1) + ") " + qAndA.answers[i] + " (" + (qAndA.results[i]*100+cumsum/2)/cumsum + "%)";
        }
        text += "\n\n<i>" + commentary + "</i>";
        surveyText.text = text;
        pieChart.SetValues(qAndA.results, qAndA.labels);
    }
}
