using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RmsAuto.TechDoc.Cache
{
	public sealed class EntityKey
	{
		private object[] _values;

		public EntityKey(params object[] values)
		{
			if (values == null || values.Length == 0)
				throw new ArgumentException("");
			_values = values;
		}

		public object this[int index]
		{
			get { return _values[index]; }
		}

		public int Arity
		{
			get { return _values.Length; }
		}

		public override bool Equals(object obj)
		{
			if (obj == null)
				return false;
			if (obj.GetType() != this.GetType())
				return false;

			EntityKey other = (EntityKey)obj;

			if (this.Arity != other.Arity)
				return false;
			for (int i = 0; i < _values.Length; i++)
				if (!Equals(this[i], other[i]))
					return false;
			return true;
		}

		public override int GetHashCode()
		{
			int hashCode = 0;
			for (int i = 0; i < _values.Length; i++)
				if (_values[i] != null)
					hashCode ^= _values[i].GetHashCode();
			return hashCode;
		}

		public static implicit operator EntityKey(object[] values)
		{
			return new EntityKey(values);
		}

        public static implicit operator EntityKey(double value)
        {
            return new EntityKey( new [] { value} );
        }
	}
}
