using System;
using System.Collections.Generic;
using System.Linq;

namespace Lending.Domain
{
    //Taken from http://www.siepman.nl/blog/post/2013/10/28/ID-Sequential-Guid-COMB-Vs-Int-Identity-using-Entity-Framework.aspx
    public class SequentialGuid
    {

        public DateTime SequenceStartDate { get; private set; }
        public DateTime SequenceEndDate { get; private set; }

        private const int NumberOfBytes = 6;
        private const int PermutationsOfAByte = 256;
        private readonly long maximumPermutations = (long)Math.Pow(PermutationsOfAByte, NumberOfBytes);
        private long lastSequence;

        public SequentialGuid(DateTime sequenceStartDate, DateTime sequenceEndDate)
        {
            SequenceStartDate = sequenceStartDate;
            SequenceEndDate = sequenceEndDate;
        }

        public SequentialGuid()
            : this(new DateTime(2011, 10, 15), new DateTime(2100, 1, 1))
        {
        }

        private static readonly Lazy<SequentialGuid> InstanceField = new Lazy<SequentialGuid>(() => new SequentialGuid());
        internal static SequentialGuid Instance => InstanceField.Value;

        public static Guid NewGuid()
        {
            return Instance.GetGuid();
        }

        public TimeSpan TimePerSequence
        {
            get
            {
                var ticksPerSequence = TotalPeriod.Ticks / maximumPermutations;
                var result = new TimeSpan(ticksPerSequence);
                return result;
            }
        }

        public TimeSpan TotalPeriod
        {
            get
            {
                var result = SequenceEndDate - SequenceStartDate;
                return result;
            }
        }

        private long GetCurrentSequence(DateTime value)
        {
            var ticksUntilNow = value.Ticks - SequenceStartDate.Ticks;
            var result = ((decimal)ticksUntilNow / TotalPeriod.Ticks * maximumPermutations);
            return (long)result;
        }

        public Guid GetGuid()
        {
            return GetGuid(DateTime.Now);
        }

        private readonly object synchronizationObject = new object();
        internal Guid GetGuid(DateTime now)
        {
            if (now < SequenceStartDate || now >= SequenceEndDate)
            {
                return Guid.NewGuid(); // Outside the range, use regular Guid
            }

            var sequence = GetCurrentSequence(now);
            return GetGuid(sequence);
        }

        internal Guid GetGuid(long sequence)
        {
            lock (synchronizationObject)
            {
                if (sequence <= lastSequence)
                {
                    // Prevent double sequence on same server
                    sequence = lastSequence + 1;
                }
                lastSequence = sequence;
            }

            var sequenceBytes = GetSequenceBytes(sequence);
            var guidBytes = GetGuidBytes();
            var totalBytes = guidBytes.Concat(sequenceBytes).ToArray();
            var result = new Guid(totalBytes);
            return result;
        }

        private IEnumerable<byte> GetSequenceBytes(long sequence)
        {
            var sequenceBytes = BitConverter.GetBytes(sequence);
            var sequenceBytesLongEnough = sequenceBytes.Concat(new byte[NumberOfBytes]);
            var result = sequenceBytesLongEnough.Take(NumberOfBytes).Reverse();
            return result;
        }

        private IEnumerable<byte> GetGuidBytes()
        {
            var result = Guid.NewGuid().ToByteArray().Take(10).ToArray();
            return result;
        }
    }
}
