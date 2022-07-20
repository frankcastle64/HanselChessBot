namespace HanselChessBOT.ConsoleApp
{
    public class DirectionMasks
    {

        /*
              northwest    north   northeast
              noWe         nort         noEa
                      +7    +8    +9
                          \  |  /
              west    -1 <-  0 -> +1    east
                          /  |  \
                      -9    -8    -7
              soWe         sout         soEa
              southwest    south   southeast
         */

        public const int NorthWest = 7;
        public const int North = 8;
        public const int NorthEast = 9;
        public const int East = 1;
        public const int SouthEast = -7;
        public const int South = -8;
        public const int SouthWest = -9;
        public const int West = -1;


    }

    public static class MoveEncodingMasks
    {
        public const int SQ_TO_SHIFT = 6;
        public const int PIECE_FROM_SHIFT = 12;
        public const int PIECE_TO_SHIFT = 16;
        public const int TYPE_OF_MOVE_SHIFT = 20;
        public const int PROMOTED_PIECE_SHIFT = 23;
        public const int PIECE_TYPE_SHIFT = 27;


        public const int SQ_FROM_MASK = 0x3F;
        public const int SQ_TO_MASK = 0x3F;
        public const int PIECE_FROM_MASK = 0xF;
        public const int PIECE_TO_MASK = 0xF;
        public const int MOVE_TYPE_MASK = 0x7;
        public const int PROMOTED_PIECE_MASK = 0xF;
        public const int PIECE_TYPE_MASK = 0x7;


    }
}
