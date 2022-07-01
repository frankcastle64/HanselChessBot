﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HanselChessBOT.ConsoleApp
{
    public class AttackMaps
    {
        public static readonly ulong[] KNIGHT_ATTACKS = new ulong[] {
        0x20400UL,0x50800UL,0xa1100UL,0x142200UL,0x284400UL,0x508800UL,0xa01000UL,0x402000UL,
        0x2040004UL,0x5080008UL,0xa110011UL,0x14220022UL,0x28440044UL,0x50880088UL,0xa0100010UL,0x40200020UL,
        0x204000402UL,0x508000805UL,0xa1100110aUL,0x1422002214UL,0x2844004428UL,0x5088008850UL,0xa0100010a0UL,0x4020002040UL,
        0x20400040200UL,0x50800080500UL,0xa1100110a00UL,0x142200221400UL,0x284400442800UL,0x508800885000UL,0xa0100010a000UL,0x402000204000UL,
        0x2040004020000UL,0x5080008050000UL,0xa1100110a0000UL,0x14220022140000UL,0x28440044280000UL,0x50880088500000UL,0xa0100010a00000UL,0x40200020400000UL,
        0x204000402000000UL,0x508000805000000UL,0xa1100110a000000UL,0x1422002214000000UL,0x2844004428000000UL,0x5088008850000000UL,0xa0100010a0000000UL,0x4020002040000000UL,
        0x400040200000000UL,0x800080500000000UL,0x1100110a00000000UL,0x2200221400000000UL,0x4400442800000000UL,0x8800885000000000UL,0x100010a000000000UL,0x2000204000000000UL,
        0x4020000000000UL,0x8050000000000UL,0x110a0000000000UL,0x22140000000000UL,0x44280000000000UL,0x88500000000000UL,0x10a00000000000UL,0x20400000000000UL
        };

        public static readonly ulong[] KING_ATTACKS = new ulong[] {
        0x302UL,0x705UL,0xe0aUL,0x1c14UL,0x3828UL,0x7050UL,0xe0a0UL,0xc040UL,
        0x30203UL,0x70507UL,0xe0a0eUL,0x1c141cUL,0x382838UL,0x705070UL,0xe0a0e0UL,0xc040c0UL,
        0x3020300UL,0x7050700UL,0xe0a0e00UL,0x1c141c00UL,0x38283800UL,0x70507000UL,0xe0a0e000UL,0xc040c000UL,
        0x302030000UL,0x705070000UL,0xe0a0e0000UL,0x1c141c0000UL,0x3828380000UL,0x7050700000UL,0xe0a0e00000UL,0xc040c00000UL,
        0x30203000000UL,0x70507000000UL,0xe0a0e000000UL,0x1c141c000000UL,0x382838000000UL,0x705070000000UL,0xe0a0e0000000UL,0xc040c0000000UL,
        0x3020300000000UL,0x7050700000000UL,0xe0a0e00000000UL,0x1c141c00000000UL,0x38283800000000UL,0x70507000000000UL,0xe0a0e000000000UL,0xc040c000000000UL,
        0x302030000000000UL,0x705070000000000UL,0xe0a0e0000000000UL,0x1c141c0000000000UL,0x3828380000000000UL,0x7050700000000000UL,0xe0a0e00000000000UL,0xc040c00000000000UL,
        0x203000000000000UL,0x507000000000000UL,0xa0e000000000000UL,0x141c000000000000UL,0x2838000000000000UL,0x5070000000000000UL,0xa0e0000000000000UL,0x40c0000000000000UL
        };

        public static readonly ulong[] BISHOP_ATTACK_MASK = new ulong[] {
        0x8040201008040200UL,0x80402010080500UL,0x804020110a00UL,0x8041221400UL,0x182442800UL,0x10204885000UL,0x102040810a000UL,0x102040810204000UL,
        0x4020100804020002UL,0x8040201008050005UL,0x804020110a000aUL,0x804122140014UL,0x18244280028UL,0x1020488500050UL,0x102040810a000a0UL,0x204081020400040UL,
        0x2010080402000204UL,0x4020100805000508UL,0x804020110a000a11UL,0x80412214001422UL,0x1824428002844UL,0x102048850005088UL,0x2040810a000a010UL,0x408102040004020UL,
        0x1008040200020408UL,0x2010080500050810UL,0x4020110a000a1120UL,0x8041221400142241UL,0x182442800284482UL,0x204885000508804UL,0x40810a000a01008UL,0x810204000402010UL,
        0x804020002040810UL,0x1008050005081020UL,0x20110a000a112040UL,0x4122140014224180UL,0x8244280028448201UL,0x488500050880402UL,0x810a000a0100804UL,0x1020400040201008UL,
        0x402000204081020UL,0x805000508102040UL,0x110a000a11204080UL,0x2214001422418000UL,0x4428002844820100UL,0x8850005088040201UL,0x10a000a010080402UL,0x2040004020100804UL,
        0x200020408102040UL,0x500050810204080UL,0xa000a1120408000UL,0x1400142241800000UL,0x2800284482010000UL,0x5000508804020100UL,0xa000a01008040201UL,0x4000402010080402UL,
        0x2040810204080UL,0x5081020408000UL,0xa112040800000UL,0x14224180000000UL,0x28448201000000UL,0x50880402010000UL,0xa0100804020100UL,0x40201008040201UL

        };

        public static readonly ulong[] ROOKS_ATTACK_MASK = new ulong[]
        {
        0x1010101010101feUL,0x2020202020202fdUL,0x4040404040404fbUL,0x8080808080808f7UL,0x10101010101010efUL,0x20202020202020dfUL,0x40404040404040bfUL,0x808080808080807fUL,
        0x10101010101fe01UL,0x20202020202fd02UL,0x40404040404fb04UL,0x80808080808f708UL,0x101010101010ef10UL,0x202020202020df20UL,0x404040404040bf40UL,0x8080808080807f80UL,
        0x101010101fe0101UL,0x202020202fd0202UL,0x404040404fb0404UL,0x808080808f70808UL,0x1010101010ef1010UL,0x2020202020df2020UL,0x4040404040bf4040UL,0x80808080807f8080UL,
        0x1010101fe010101UL,0x2020202fd020202UL,0x4040404fb040404UL,0x8080808f7080808UL,0x10101010ef101010UL,0x20202020df202020UL,0x40404040bf404040UL,0x808080807f808080UL,
        0x10101fe01010101UL,0x20202fd02020202UL,0x40404fb04040404UL,0x80808f708080808UL,0x101010ef10101010UL,0x202020df20202020UL,0x404040bf40404040UL,0x8080807f80808080UL,
        0x101fe0101010101UL,0x202fd0202020202UL,0x404fb0404040404UL,0x808f70808080808UL,0x1010ef1010101010UL,0x2020df2020202020UL,0x4040bf4040404040UL,0x80807f8080808080UL,
        0x1fe010101010101UL,0x2fd020202020202UL,0x4fb040404040404UL,0x8f7080808080808UL,0x10ef101010101010UL,0x20df202020202020UL,0x40bf404040404040UL,0x807f808080808080UL,
        0xfe01010101010101UL,0xfd02020202020202UL,0xfb04040404040404UL,0xf708080808080808UL,0xef10101010101010UL,0xdf20202020202020UL,0xbf40404040404040UL,0x7f80808080808080UL,
        };


        public static readonly ulong[,] PAWN_ATTACK_MASK = new ulong[,] { 

            // White pawn attack mask.
            {
               0x200UL,0x500UL,0xa00UL,0x1400UL,0x2800UL,0x5000UL,0xa000UL,0x4000UL,
               0x20000UL,0x50000UL,0xa0000UL,0x140000UL,0x280000UL,0x500000UL,0xa00000UL,0x400000UL,
               0x2000000UL,0x5000000UL,0xa000000UL,0x14000000UL,0x28000000UL,0x50000000UL,0xa0000000UL,0x40000000UL,
               0x200000000UL,0x500000000UL,0xa00000000UL,0x1400000000UL,0x2800000000UL,0x5000000000UL,0xa000000000UL,0x4000000000UL,
               0x20000000000UL,0x50000000000UL,0xa0000000000UL,0x140000000000UL,0x280000000000UL,0x500000000000UL,0xa00000000000UL,0x400000000000UL,
               0x2000000000000UL,0x5000000000000UL,0xa000000000000UL,0x14000000000000UL,0x28000000000000UL,0x50000000000000UL,0xa0000000000000UL,0x40000000000000UL,
               0x200000000000000UL,0x500000000000000UL,0xa00000000000000UL,0x1400000000000000UL,0x2800000000000000UL,0x5000000000000000UL,0xa000000000000000UL,0x4000000000000000UL,
               0x0UL,0x0UL,0x0UL,0x0UL,0x0UL,0x0UL,0x0UL,0x0UL

            },


            // Black pawn attack mask.
            {
                0x0UL,0x0UL,0x0UL,0x0UL,0x0UL,0x0UL,0x0UL,0x0UL,
                0x2UL,0x5UL,0xaUL,0x14UL,0x28UL,0x50UL,0xa0UL,0x40UL,
                0x200UL,0x500UL,0xa00UL,0x1400UL,0x2800UL,0x5000UL,0xa000UL,0x4000UL,
                0x20000UL,0x50000UL,0xa0000UL,0x140000UL,0x280000UL,0x500000UL,0xa00000UL,0x400000UL,
                0x2000000UL,0x5000000UL,0xa000000UL,0x14000000UL,0x28000000UL,0x50000000UL,0xa0000000UL,0x40000000UL,
                0x200000000UL,0x500000000UL,0xa00000000UL,0x1400000000UL,0x2800000000UL,0x5000000000UL,0xa000000000UL,0x4000000000UL,
                0x20000000000UL,0x50000000000UL,0xa0000000000UL,0x140000000000UL,0x280000000000UL,0x500000000000UL,0xa00000000000UL,0x400000000000UL,
                0x2000000000000UL,0x5000000000000UL,0xa000000000000UL,0x14000000000000UL,0x28000000000000UL,0x50000000000000UL,0xa0000000000000UL,0x40000000000000UL
            }

        };

        public static readonly ulong[] BLACK_PAWN_ATTACK_MASK = new ulong[]
        {
            0x0UL,0x0UL,0x0UL,0x0UL,0x0UL,0x0UL,0x0UL,0x0UL,
                0x2UL,0x5UL,0xaUL,0x14UL,0x28UL,0x50UL,0xa0UL,0x40UL,
                0x200UL,0x500UL,0xa00UL,0x1400UL,0x2800UL,0x5000UL,0xa000UL,0x4000UL,
                0x20000UL,0x50000UL,0xa0000UL,0x140000UL,0x280000UL,0x500000UL,0xa00000UL,0x400000UL,
                0x2000000UL,0x5000000UL,0xa000000UL,0x14000000UL,0x28000000UL,0x50000000UL,0xa0000000UL,0x40000000UL,
                0x200000000UL,0x500000000UL,0xa00000000UL,0x1400000000UL,0x2800000000UL,0x5000000000UL,0xa000000000UL,0x4000000000UL,
                0x20000000000UL,0x50000000000UL,0xa0000000000UL,0x140000000000UL,0x280000000000UL,0x500000000000UL,0xa00000000000UL,0x400000000000UL,
                0x2000000000000UL,0x5000000000000UL,0xa000000000000UL,0x14000000000000UL,0x28000000000000UL,0x50000000000000UL,0xa0000000000000UL,0x40000000000000UL
        };

        public static readonly ulong[] WHITE_PAWN_ATTACK_MASK = new ulong[]
        {
            0x200UL,0x500UL,0xa00UL,0x1400UL,0x2800UL,0x5000UL,0xa000UL,0x4000UL,
               0x20000UL,0x50000UL,0xa0000UL,0x140000UL,0x280000UL,0x500000UL,0xa00000UL,0x400000UL,
               0x2000000UL,0x5000000UL,0xa000000UL,0x14000000UL,0x28000000UL,0x50000000UL,0xa0000000UL,0x40000000UL,
               0x200000000UL,0x500000000UL,0xa00000000UL,0x1400000000UL,0x2800000000UL,0x5000000000UL,0xa000000000UL,0x4000000000UL,
               0x20000000000UL,0x50000000000UL,0xa0000000000UL,0x140000000000UL,0x280000000000UL,0x500000000000UL,0xa00000000000UL,0x400000000000UL,
               0x2000000000000UL,0x5000000000000UL,0xa000000000000UL,0x14000000000000UL,0x28000000000000UL,0x50000000000000UL,0xa0000000000000UL,0x40000000000000UL,
               0x200000000000000UL,0x500000000000000UL,0xa00000000000000UL,0x1400000000000000UL,0x2800000000000000UL,0x5000000000000000UL,0xa000000000000000UL,0x4000000000000000UL,
               0x0UL,0x0UL,0x0UL,0x0UL,0x0UL,0x0UL,0x0UL,0x0UL
        };
    }
}
