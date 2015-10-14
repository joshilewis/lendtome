using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public abstract class Fixture
    {
        [SetUp]
        public virtual void SetUp()
        {
            
        }

        [TearDown]
        public virtual void TearDown()
        {
            
        }

    }
}
