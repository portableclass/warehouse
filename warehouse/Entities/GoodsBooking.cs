//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace warehouse.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class GoodsBooking
    {
        public int GoodsBookingId { get; set; }
        public Nullable<int> GoodsId { get; set; }
        public Nullable<int> BookingId { get; set; }
        public Nullable<int> CustomerId { get; set; }
        public Nullable<int> GoodsBookingAmount { get; set; }
    
        public virtual Booking Booking { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual Goods Goods { get; set; }
    }
}
