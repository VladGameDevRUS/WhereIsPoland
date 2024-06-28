using UnityEngine;

[CreateAssetMenu(fileName = "NewQuestiondata", menuName = "QuestionData")]
public class QuizData : ScriptableObject
{
    [System.Serializable]
   
    public struct Question
    {
        public string questionText;
        public string[] replies;
        public int correctReplyIndex;
        public Sprite sprite;
    }
    public Question[] questions;
}

