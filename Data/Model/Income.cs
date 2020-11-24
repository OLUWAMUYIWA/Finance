using System;

namespace ListaccFinance.API.Data.Model
{
    public class Income
    {
        public int Id {get;set;}
        public string Type {get;set;}
        public DateTime Date {get;set;}
        public int ServiceId {get;set;}
        public Service Service {get;set;}
        public int ClientId {get;set;}
        public Client Client {get;set;}
        public double AmountReceived {get;set;}
        public double Discount {get;set;}
        public string PaymentType {get;set;}
        public double AmountReceivable {get;set;}
        public DateTime DateDue {get;set;}
        public int Unit {get; set;}
        public int IncomeId {get; set;}
        public int UserId {get;set;}
        public User User {get;set;}

    }

}