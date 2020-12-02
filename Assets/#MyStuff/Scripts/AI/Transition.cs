namespace NonViolentFPS.AI
{
    [System.Serializable]
    public class Transition
    {
        public Condition condition;
        public State trueState;
        public State falseState;
    }
}