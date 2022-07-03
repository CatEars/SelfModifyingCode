using System.Collections.Immutable;

namespace SelfModifyingCode.Server.Directory;

public record ProgramDirectory(IImmutableList<Program> Programs)
{

    public static ProgramDirectory Empty() => new(ImmutableList<Program>.Empty);

    public ProgramDirectory With(Program program) => this with
    {
        Programs = Programs.Add(program)
    };

}