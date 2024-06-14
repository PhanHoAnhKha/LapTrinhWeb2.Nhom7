Create database JobAPI
Go
Use JobAPI


  -- Xóa tất cả dữ liệu trong bảng
   DELETE FROM Jobs;

   -- Đặt lại giá trị tự tăng (IDENTITY) về 1
   DBCC CHECKIDENT ('Jobs', RESEED, 0);


   -- Xóa tất cả dữ liệu trong bảng
   DELETE FROM Staffs;

   -- Đặt lại giá trị tự tăng (IDENTITY) về 1
   DBCC CHECKIDENT ('Staffs', RESEED, 0);


