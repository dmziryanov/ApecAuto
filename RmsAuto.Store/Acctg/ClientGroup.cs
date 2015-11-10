using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RmsAuto.Acctg
{
    public enum ClientGroup
    {
        Group1 = 1,
        Group2,
        Group3,
        Group4,
        Group5,
        Group6,
        Group7,
        Group8,
        Group9,
        Group10,
        Group11,
        Group12,
        Group13,
        Group14,
        Group15,
		Group16,
		Group17,
		Group18,
		Group19,
		Group20,
		Group21,
		Group22,
		Group23,
		Group24,
		Group25,
        Group26,
        Group27,
        Group28,
        Group29,
        Group30,
        Group31,
        Group32,
        Group33,
        Group34,
        Group35,
        Group36,
        Group37,
        Group38,
        Group39,
        Group40,
        Group41,
        Group42,
        Group43,
        Group44,
        Group45,
        Group46,
        Group47,
        Group48,
        Group49,
        Group50,
        Group51,
        Group52,
        Group53,
        Group54,
        Group55,
        Group56,
        Group57,
        Group58,
        Group59,
        Group60,
        Group61,
        Group62,
        Group63,
        Group64,
        Group65,
        Group66,
        Group67,
        Group68,
        Group69,
        Group70,
        Group71,
        Group72,
        Group73,
        Group74,
        Group75,
        Group76,
        Group77,
        Group78,
        Group79,
        Group80,
        Group81,
        Group82,
        Group83,
        Group84,
        Group85,
        Group86,
        Group87,
        Group88,
        Group89,
        Group90,
        Group91,
        Group92,
        Group93,
        Group94,
        Group95,
        Group96,
        Group97,
        Group98,
        Group99,
        Group100,
        /// <summary>
        /// default unauthorized client group
        /// </summary>
		NonAuthorized = Group13,
        /// <summary>
        /// default retail client group
        /// </summary>
		DefaultRetail = Group13,
        /// <summary>
        /// default wholesale client group
        /// </summary>
        DefaultWholesale = Group13
    }

    public static class ClientGroupUtil
    {
        public static int GetIndex(this ClientGroup cg)
        {
            return (int)cg - 1;
        }
    }
}
