using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PicSimu.Model
{
    class Programmspeicher
    {
        private int[] storageField;
        private int numberOfValuesInStorage;

        public Programmspeicher()
        {
            storageField = new int[1024];
            numberOfValuesInStorage = 0;
        }

        public void newValueInProgramStorage(int value, int ind)
        {
            storageField[ind] = value;
            numberOfValuesInStorage++;
        }

        public int getStorageField(int ind)
        {
            return storageField[ind];
        }

        public int getNumberOfValuesInStorage()
        {
            return numberOfValuesInStorage;
        }

    }
}
