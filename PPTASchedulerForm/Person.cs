using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPTASchedulerForm
{
    public abstract class Person
    {
        private string firstName;
        private string middleName;
        private string lastName;
        private int Id;

        public Person(string firstName, string middleName, string lastName, int Id) : this(firstName, lastName, Id)
        {
            this.middleName = middleName;
        }

        public Person(string firstName, string lastName, int Id)
        {
            this.firstName = firstName;
            this.lastName = lastName;
            this.Id = Id;
        }

        public string FirstName {
            get
            {
                return firstName;
            }
        }

        public string MiddleName {
            get
            {
                return middleName;
            }
        }

        public string LastName
        {
            get
            {
                return lastName;
            }
        }

        public int ID
        {
            get
            {
                return Id;
            }
        }

        public abstract override string ToString();

    }
}
