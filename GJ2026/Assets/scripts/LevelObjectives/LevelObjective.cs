namespace LevelObjectives
{
    public class LevelObjective
    {
        private readonly string inProgressMessage;
        private readonly string inProgressEmoji;

        private readonly string completionMessage;
        private readonly string completionEmoji;

        private readonly int progressRequired;
        private int currentProgress = 0;

        public LevelObjective(
            string inProgressMessage, string inProgressEmoji,
            string completionMessage, string completionEmoji,
            int progressRequired)
        {
            this.inProgressMessage = inProgressMessage;
            this.inProgressEmoji = inProgressEmoji;
            this.completionMessage = completionMessage;
            this.completionEmoji = completionEmoji;
            this.progressRequired = progressRequired;
        }

        public bool IsChallengeComplete => currentProgress >= progressRequired;

        public bool TryIncrementProgress()
        {
            if (!IsChallengeComplete)
            {
                currentProgress += 1;
                return true;
            }
            return false;
        }

        public string GetCurrentMessage()
        {
            return IsChallengeComplete && completionMessage != null
                ? completionMessage : inProgressMessage;
        }

        public string GetCurrentEmoji()
        {
            return IsChallengeComplete && completionMessage != null
                ? completionEmoji : inProgressEmoji;
        }

        public string GetProgressText()
        {
            if (GetCurrentEmoji() != null && !IsChallengeComplete)
            {
                if (progressRequired != 0)
                {
                    return $"{currentProgress}/{progressRequired}";
                }
                return string.Empty;
            }
            return null;
        }
    }
}