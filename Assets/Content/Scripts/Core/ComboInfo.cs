using System;

namespace Content.Scripts.Core
{
    [Serializable]
    public struct ComboInfo
    {
        public int firstDiceResult;
        public int secondDiceResult;
        public int thirdDiceResult;

        public ComboInfo(int firstDiceResult, int secondDiceResult, int thirdDiceResult)
        {
            this.firstDiceResult = firstDiceResult;
            this.secondDiceResult = secondDiceResult;
            this.thirdDiceResult = thirdDiceResult;
        }

        public override string ToString()
        {
            return $"First:{firstDiceResult}. Second:{secondDiceResult}. Third:{thirdDiceResult}.";
        }

        public bool HasNumber(int targetNumber)
        {
            if (firstDiceResult == targetNumber)
            {
                return true;
            }

            if (secondDiceResult == targetNumber)
            {
                return true;
            }

            if (thirdDiceResult == targetNumber)
            {
                return true;
            }

            return false;
        }

        public bool IsTriple()
        {
            if (firstDiceResult == secondDiceResult && secondDiceResult == thirdDiceResult)
            {
                return true;
            }

            return false;
        }

        public bool IsSetPointCombo()
        {
            if (IsTriple())
            {
                return false;
            }
            return firstDiceResult == secondDiceResult || secondDiceResult == thirdDiceResult || firstDiceResult == thirdDiceResult;
        }

        public int GetSetPointNumber()
        {
            if (firstDiceResult == secondDiceResult)
            {
                return thirdDiceResult;
            }

            if (secondDiceResult == thirdDiceResult)
            {
                return firstDiceResult;
            }

            if (firstDiceResult == thirdDiceResult)
            {
                return secondDiceResult;
            }

            return 0;
        }
    }
}
