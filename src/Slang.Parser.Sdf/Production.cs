using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slang.Parsing;
using Virtlink.Utilib.Collections;

namespace Slang.Parser.Sdf
{
    /// <summary>
    /// An SDF production.
    /// </summary>
    public sealed class Production : IReduction
    {
        // TODO: This can be of type T
        /// <summary>
        /// Gets the reduction non-terminal on the left-hand of the production rule.
        /// </summary>
        /// <value>A non-terminal.</value>
        public ISort Symbol { get; }

        // TODO: This can be of type [T]
        /// <summary>
        /// Gets the parsing expression on the right-hand of the production rule.
        /// </summary>
        /// <value>A possibly empty list of terminals and non-terminals.</value>
        public IReadOnlyList<ISymbol> Expression { get; }

        /// <summary>
        /// Gets the constructor of the production.
        /// </summary>
        /// <value>The constructor of the production;
        /// or <see langword="null"/> when the production has no constructor.</value>
        public string Constructor { get; }

        /// <summary>
        /// Gets the type of production.
        /// </summary>
        /// <value>A member of the <see cref="ProductionType"/> enumeration.</value>
        public ProductionType Type { get; }

        /// <summary>
        /// Gets the flags that apply to the production.
        /// </summary>
        /// <value>A bitwise combination of members of the <see cref="ProductionFlags"/> enumeration.</value>
        public ProductionFlags Flags { get; }

        /// <summary>
        /// Gets the kind of reduction.
        /// </summary>
        /// <value>A member of the <see cref="ReductionKind"/> enumeration.</value>
        public ReductionKind ReductionKind { get; }

        /// <summary>
        /// Gets the sort in the reduction.
        /// </summary>
        /// <value>The sort in the reduction; or <see langword="null"/>.</value>
        public string ReductionSort { get; }

        /// <summary>
        /// Gets the arity of the production.
        /// </summary>
        /// <value>The arity of the production.</value>
        public int Arity => this.Expression.Count;

        public bool IsRecover => this.Flags.HasFlag(ProductionFlags.Recover);

        public bool IsCompletion => this.Flags.HasFlag(ProductionFlags.Completion);

        public bool IsReject => this.Type == ProductionType.Reject;

        public bool Rejects => this.IsReject;


        /// <summary>
        /// Gets whether the reduction is layout.
        /// </summary>
        /// <value><see langword="true"/> when the reduction is layout;
        /// otherwise, <see langword="false"/>.</value>
        public bool IsLayout => this.ReductionKind == ReductionKind.Layout;

        /// <summary>
        /// Gets whether the reduction is not context-free (e.g. lexical, literal, layout or a variable node).
        /// </summary>
        /// <value><see langword="true"/> when the reduction is not context-free;
        /// otherwise, <see langword="false"/>.</value>
        public bool IsNotContextFree => this.IsLexical || this.IsLiteral || this.IsLayout || this.IsVariableNode;

        /// <summary>
        /// Gets whether the reduction is a list.
        /// </summary>
        /// <value><see langword="true"/> when the reduction is a list;
        /// otherwise, <see langword="false"/>.</value>
        public bool IsList => this.IsIterable || this.IsSequence;

        public bool IsLexical => this.ReductionKind == ReductionKind.Lexical;

        public bool IsLiteral => this.ReductionKind == ReductionKind.Literal;

        public bool IsIterable => this.ReductionKind == ReductionKind.Iterable;

        public bool IsSequence => this.ReductionKind == ReductionKind.Sequence;

        public bool IsVariableNode => this.ReductionKind == ReductionKind.VariableNode;

        public bool IsLexicalLayout
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsOptional => this.ReductionKind == ReductionKind.Optional;

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Production"/> class.
        /// </summary>
        /// <param name="symbol">The left-hand symbol.</param>
        /// <param name="constructor">The constructor of the production rule; or <see langword="null"/>.</param>
        /// <param name="type">The type of production.</param>
        /// <param name="flags">The production flags.</param>
        public Production(ISort symbol, IReadOnlyList<ISymbol> expression, string constructor, ProductionType type, ProductionFlags flags)
        {
            #region Contract
            if (symbol == null)
                throw new ArgumentNullException(nameof(symbol));
            if (!Enum.IsDefined(typeof(ProductionType), type))
                throw new InvalidEnumArgumentException(nameof(type), (int)type, typeof(ProductionType));
            #endregion

                // TODO: Give ReductionKind a value!
                // TODO: Give ReductionSort a value!
            this.Symbol = symbol;
            this.Expression = expression;
            this.Constructor = constructor;
            this.Type = type;
            this.Flags = flags;
        }
        #endregion

        #region Equality
        /// <inheritdoc />
        public bool Equals(Production other)
        {
            if (Object.ReferenceEquals(other, null) ||      // When 'other' is null
                other.GetType() != this.GetType())          // or of a different type
                return false;                               // they are not equal.
            return this.Symbol == other.Symbol
                && this.Arity == other.Arity
                //                && ListComparer<ISymbol>.Default.Equals(this.Expression, other.Expression)
                && this.Constructor == other.Constructor
                && this.Type == other.Type
                && this.Flags == other.Flags;
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            int hash = 17;
            unchecked
            {
                hash = hash * 29 + this.Symbol.GetHashCode();
                hash = hash * 29 + this.Arity.GetHashCode();
//                hash = hash * 29 + ListComparer<ISymbol>.Default.GetHashCode(this.Expression);
                hash = hash * 29 + this.Constructor?.GetHashCode() ?? 0;
                hash = hash * 29 + this.Type.GetHashCode();
                hash = hash * 29 + this.Flags.GetHashCode();
            }
            return hash;
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            return Equals(obj as Production);
        }

        /// <summary>
        /// Returns a value that indicates whether two specified <see cref="Production"/> objects are equal.
        /// </summary>
        /// <param name="left">The first object to compare.</param>
        /// <param name="right">The second object to compare.</param>
        /// <returns><see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> are equal;
        /// otherwise, <see langword="false"/>.</returns>
        public static bool operator ==(Production left, Production right)
        {
            return Object.Equals(left, right);
        }

        /// <summary>
        /// Returns a value that indicates whether two specified <see cref="Production"/> objects are not equal.
        /// </summary>
        /// <param name="left">The first object to compare.</param>
        /// <param name="right">The second object to compare.</param>
        /// <returns><see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> are not equal;
        /// otherwise, <see langword="false"/>.</returns>
        public static bool operator !=(Production left, Production right)
        {
            return !(left == right);
        }
        #endregion

        /// <inheritdoc />
        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.Append(this.Symbol);
            if (this.Constructor != null)
                sb.Append(".").Append(this.Constructor);

            sb.Append(" -> ");
            sb.Append(String.Join("", this.Expression));
            if (this.Type != ProductionType.None || this.Flags != ProductionFlags.None)
            {
                sb.Append(" {");

                if (this.Type != ProductionType.None)
                {
                    var name = Enum.GetName(typeof(ProductionType), this.Type) ?? "??";
                    sb.Append(name.ToLowerInvariant());
                }

                if (this.Flags != ProductionFlags.None)
                {
                    if (this.Type != ProductionType.None)
                        sb.Append(", ");
                    sb.Append(String.Join(", ", this.Flags.ToString().Split(new[] { " | " }, StringSplitOptions.RemoveEmptyEntries)));
                }

                sb.Append("}");
            }

            return sb.ToString();
        }
    }
}
