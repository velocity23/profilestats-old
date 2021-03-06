using System;
using System.Collections.Generic;
using System.Text;

namespace InfiniteFlightLiveClient
{
    public static class Groups
    {
#pragma warning disable CA2211 // Non-constant fields should not be visible
        public static Guid Ifatc = new Guid("DF0F6341-5F6A-40EF-8B73-087A0EC255B5");
        public static Guid Moderators = new Guid("8C93A113-0C6C-491F-926D-1361E43A5833");
        public static Guid Staff = new Guid("D07AFAD8-79DF-4363-B1C7-A5A1DDE6E3C8");
#pragma warning restore CA2211 // Non-constant fields should not be visible
    }
}
