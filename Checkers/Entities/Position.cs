using Checkers.Enums;

namespace Checkers.Entities
{
    public class Position
    {
        private VerticalOrientation _verticalOrientation;
        private HorizontalOrientation _horizontalOrientation;

        public Position(VerticalOrientation verticalOrientation, HorizontalOrientation horizontalOrientation)
        {
            _verticalOrientation = verticalOrientation;
            _horizontalOrientation = horizontalOrientation;
        }

        public VerticalOrientation GetVerticalOrientation()
        {
            return _verticalOrientation;
        }
        public HorizontalOrientation GetHorizontalOrientation()
        {
            return _horizontalOrientation;
        }

    }
}
