using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreProcesser
{
    internal static class AFTER_EXTRA_Helper
    {
#nullable enable
        internal static readonly Dictionary<string, List<string>?> RuningProcessFiles = new()
        {
            {"CommandOutput.hpp" ,null}
            ,
            {
                "CommandRegistry.hpp",
                new()
                {
                    "public:",
                    "    struct ParseTable;",
                    "    class Symbol {",
                    "    public:",
                    "        int val;",
                    "        MCAPI Symbol(unsigned __int64 = -1);",
                    "        MCAPI Symbol(class Symbol const&);",
                    "        MCAPI unsigned __int64 toIndex() const;",
                    "        MCAPI int value() const;",
                    "        inline bool operator==(Symbol const& right) const {\r\n            return val == right.val;\r\n        }",
                    "        [[nodiscard]] inline std::string toString() const {\r\n            return Global<CommandRegistry>->symbolToString(*this);\r\n        }",
                    "        [[nodiscard]] inline std::string toDebugString() const {\r\n            return fmt::format(\"<Symbol {}({})>\", toString(), val);\r\n        }",
                    "    };",
                    "",
                    "    struct ParseToken {",
                    "        std::unique_ptr<CommandRegistry::ParseToken> child;",
                    "        std::unique_ptr<CommandRegistry::ParseToken> next;",
                    "        CommandRegistry::ParseToken* parent;",
                    "        const char* text;",
                    "        uint32_t length;",
                    "        Symbol type;",
                    "        MCAPI std::string toString() const;",
                    "        [[nodiscard]] inline std::string toDebugString() const {\r\n            return fmt::format(\"<ParseToken {}>\", toString());\r\n        }",
                    "    };",
                    "    static_assert(sizeof(ParseToken) == 40);",
                    " using ParseFn = bool (CommandRegistry::*)(void*, ParseToken const&, CommandOrigin const&, int, std::string&, std::vector<std::string>&) const;",
                    "",
                    "    struct Overload {",
                    "        using FactoryFn = std::unique_ptr<class Command> (*)();",
                    "",
                    "        CommandVersion version;",
                    "        FactoryFn factory;",
                    "        std::vector<CommandParameterData> params;",
                    "        unsigned char unk;",
                    "        std::vector<Symbol> syms = {};",
                    "        LIAPI Overload(CommandVersion version,\r\n                       FactoryFn factory,\r\n                       std::vector<CommandParameterData>&& args);",
                    "        LIAPI ~Overload();",
                    "        inline std::string toDebugString() {\r\n            return fmt::format(\"<Overload>\");\r\n        }",
                    "",
                    "    struct Signature {",
                    "        std::string name;",
                    "        std::string desc;",
                    "        std::vector<Overload> overloads;",
                    "        CommandPermissionLevel perm;",
                    "        Symbol main_symbol;",
                    "        Symbol alt_symbol;",
                    "        CommandFlag flag;",
                    "        int firstRule;",
                    "        int firstFactorization;",
                    "        int firstOptional;",
                    "        bool runnable;",
                    "        inline Signature(std::string_view name,\r\n                         std::string_view desc,\r\n                         CommandPermissionLevel perm,",
                    "                         Symbol symbol,\r\n                         CommandFlag flag)",
                    "        : name(name), desc(desc), perm(perm), main_symbol(symbol), flag(flag) {}"
                } }
        };
    }
}
