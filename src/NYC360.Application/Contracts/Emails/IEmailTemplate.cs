namespace NYC360.Application.Contracts.Emails;

public interface IEmailTemplate<T>
{
    string Subject { get; }
    string GenerateBody(T model);
}