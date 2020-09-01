using System;
using System.Collections.Generic;
using System.Text;

namespace Sample
{
    /// <summary>
    /// Entity Framework Domain object for a basic material. Defines a material's category and default values true for all instances/models/versions of a material.
    /// </summary>
    public class Material
    {
        protected Material()
        {
        }

        public int Id { get; set; }
        public bool Active { get; private set; } = true;
        public string Name { get; private set; }
        public string ContainerName { get; private set; }
        public decimal DefaultMarkup { get; private set; }
        public decimal DefaultLaborRate { get; private set; }

        // Audit columns
        public DateTime CreatedDate { get; private set; }
        public DateTime? UpdatedDate { get; private set; }
        public DateTime? DeletedDate { get; private set; }
        public int CreatedBy { get; private set; }
        public int? UpdatedBy { get; private set; }
        public int? DeletedBy { get; private set; }
        public bool Deleted { get; private set; }

        protected void SetCreateAuditInformation(int createdByUserId)
        {
            CreatedBy = createdByUserId;
            CreatedDate = DateTime.Now;
        }

        internal void SetUpdateAuditInformation(int updatedByUserId, DateTime updatedDate)
        {
            UpdatedBy = updatedByUserId;
            UpdatedDate = updatedDate;
        }
    }
}
