using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiveCharts.Configurations
{
    /// <summary>
    /// Mapper to configure From ,To and Y points
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ColumnRangeMapper<T> : IPointEvaluator<T>
    {
        private Func<T, int, double> _from = (v, i) => i;
        private Func<T, int, double> _to = (v, i) => i;
        private Func<T, int, double> _y = (v, i) => i;
        private Func<T, int, object> _stroke;
        private Func<T, int, object> _fill;

        /// <summary>
        /// Sets values for a specific point
        /// </summary>
        /// <param name="point">Point to set</param>
        /// <param name="value"></param>
        /// <param name="key"></param>
        public void Evaluate(int key, T value, ChartPoint point)
        {
            point.From = _from(value, key);
            point.To = _to(value, key);
            point.Y = _y(value, key);
            if (_stroke != null) point.Stroke = _stroke(value, key);
            if (_fill != null) point.Fill = _fill(value, key);
        }

        /// <summary>
        /// Sets the X mapper
        /// </summary>
        /// <param name="predicate">function that pulls X coordinate</param>
        /// <returns>current mapper instance</returns>
        public ColumnRangeMapper<T> From(Func<T, double> predicate)
        {
            return From((t, i) => predicate(t));
        }

        /// <summary>
        /// Sets the X mapper
        /// </summary>
        /// <param name="predicate">function that pulls X coordinate, with value and index as parameters</param>
        /// <returns>current mapper instance</returns>
        public ColumnRangeMapper<T> From(Func<T, int, double> predicate)
        {
            _from = predicate;
            return this;
        }

        /// <summary>
        /// Sets the X mapper
        /// </summary>
        /// <param name="predicate">function that pulls X coordinate</param>
        /// <returns>current mapper instance</returns>
        public ColumnRangeMapper<T> To(Func<T, double> predicate)
        {
            return To((t, i) => predicate(t));
        }

        /// <summary>
        /// Sets the X mapper
        /// </summary>
        /// <param name="predicate">function that pulls X coordinate, with value and index as parameters</param>
        /// <returns>current mapper instance</returns>
        public ColumnRangeMapper<T> To(Func<T, int, double> predicate)
        {
            _to = predicate;
            return this;
        }
        /// <summary>
        /// Sets the Y mapper
        /// </summary>
        /// <param name="predicate">function that pulls Y coordinate</param>
        /// <returns>current mapper instance</returns>
        public ColumnRangeMapper<T> Y(Func<T, double> predicate)
        {
            return Y((t, i) => predicate(t));
        }

        /// <summary>
        /// Sets the Y mapper
        /// </summary>
        /// <param name="predicate">function that pulls Y coordinate, with value and index as parameters</param>
        /// <returns>current mapper instance</returns>
        public ColumnRangeMapper<T> Y(Func<T, int, double> predicate)
        {
            _y = predicate;
            return this;
        }

        /// <summary>
        /// Sets the Stroke of the point
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public ColumnRangeMapper<T> Stroke(Func<T, object> predicate)
        {
            return Stroke((t, i) => predicate(t));
        }

        /// <summary>
        /// Sets the Stroke of the point
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public ColumnRangeMapper<T> Stroke(Func<T, int, object> predicate)
        {
            _stroke = predicate;
            return this;
        }

        /// <summary>
        /// Sets the Fill of the point
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public ColumnRangeMapper<T> Fill(Func<T, object> predicate)
        {
            return Fill((t, i) => predicate(t));
        }

        /// <summary>
        /// Sets the Fill of the point
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public ColumnRangeMapper<T> Fill(Func<T, int, object> predicate)
        {
            _fill = predicate;
            return this;
        }
    }
}
