﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaleIt.Domain.Sale.Entities
{
    public class SaleDocument
    {
        private SaleDocument()
        {
            // required for orm
        }

        private SaleDocument(Guid saleId)
        {
            // required for orm delete
        }

        public SaleDocument(Guid saleId, string documentNo, DateTime documentDate, Guid customerId)
        {
            this.saleId = saleId;
            this.documentNo = documentNo;
            this.documentDate = documentDate;
            this.lines = new List<SaleDocumentLine>();
            this.description = string.Empty;
        }

        private Guid saleId;
        public Guid SaleId => saleId;

        private Guid customerId;
        public Guid CustomerId => customerId;

        private string documentNo;
        public string DocumentNo => documentNo;

        private string description;
        public string Description
        {
            get
            {
                return description;
            }
            set
            {
                description = value;
            }
        }

        private DateTime documentDate;
        public DateTime DocumentDate => documentDate;

        private List<SaleDocumentLine> lines;
        public IReadOnlyCollection<SaleDocumentLine> Lines => lines;

        public void AddLine(SaleDocumentLine line)
        {
            lines.Add(line);
        }

        public void AddLines(IEnumerable<SaleDocumentLine> lines)
        {
            this.lines.AddRange(lines);
        }

        public void ChangeDocumentDate(DateTime date)
        {
             documentDate = date;
        }

        public bool HasCustomer()
        {
            return customerId != Guid.Empty;
        }
        public decimal Total => Lines.Sum(l => l.Amount);

        #region Factory Methods

        public static SaleDocument CreateForDelete(Guid saleId)
        {
            return new SaleDocument(saleId);
        }

        #endregion
    }
}
