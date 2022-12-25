using System.Text.Json.Serialization;

namespace LibraryGenerator.Utils
{
    internal struct OriginalData
    {
        public Dictionary<string, Dictionary<string, List<Item>>> classes { get; set; }
        public struct Item
        {
            public int access_type { get; set; }
            public string fake_symbol { get; set; }
            public int flag_bits { get; set; }
            public string name { get; set; }
            public string @namespace { get; set; }
            public List<Type> @params { get; set; }
            public struct Type
            {
                public int kind { get; set; }
                public string name { get; set; }
            }
            public int rva { get; set; }
            public int storage_class { get; set; }
            public string symbol { get; set; }
            public int symbol_type { get; set; }
            public Type type { get; set; }
        }
        public Dictionary<string, int> fn_list { get; set; }
        public Identifier identifier { get; set; }
        public struct Identifier
        {
            public List<string> @class { get; set; }
            public List<string> @struct { get; set; }
        }
        public Hashs sha_256_hash { get; set; }
        public struct Hashs
        {
            [JsonPropertyName("bedrock_server.exe")]
            public string exe { get; set; }
            [JsonPropertyName("bedrock_server.pdb")]
            public string pdb { get; set; }
        }
    }
}
