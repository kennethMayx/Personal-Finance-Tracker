// Abstract Transaction Class
public abstract class Transaction
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public double Amount { get; set; }
    public DateTime Date { get; set; }
    public string Description { get; set; }

    public abstract override string ToString();
}

// Income Class
public class Income : Transaction
{
    public override string ToString()
    {
        return $"[Income] ${Amount} - {Description} on {Date.ToShortDateString()}";
    }
}

// Expense Class
public class Expense : Transaction
{
    public override string ToString()
    {
        return $"[Expense] ${Amount} - {Description} on {Date.ToShortDateString()}";
    }
}

// Interface for Report Generation
public interface IReportGenerator
{
    string GenerateReport(List<Transaction> transactions);
}

// Report Generator Implementation
public class ReportGenerator : IReportGenerator
{
    public string GenerateReport(List<Transaction> transactions)
    {
        double totalIncome = 0;
        double totalExpense = 0;

        foreach (var transaction in transactions)
        {
            if (transaction is Income)
                totalIncome += transaction.Amount;
            else if (transaction is Expense)
                totalExpense += transaction.Amount;
        }

        double balance = totalIncome - totalExpense;

        return $"Total Income: ${totalIncome}\nTotal Expenses: ${totalExpense}\nNet Balance: ${balance}";
    }
}

// User Class
public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public List<Transaction> Transactions { get; set; } = new List<Transaction>();

    public void AddTransaction(Transaction transaction)
    {
        Transactions.Add(transaction);
    }

    public List<Transaction> GetTransactions()
    {
        return Transactions;
    }
}

// FinanceManager Class
public class FinanceManager
{
    private List<Transaction> transactions = new List<Transaction>();
    private IReportGenerator reportGenerator = new ReportGenerator();

    public void AddTransaction(Transaction transaction)
    {
        transactions.Add(transaction);
    }

    public void DeleteTransaction(int id)
    {
        transactions.RemoveAll(t => t.Id == id);
    }

    public string GenerateReport()
    {
        return reportGenerator.GenerateReport(transactions);
    }

    public List<Transaction> GetAllTransactions()
    {
        return transactions;
    }
}
