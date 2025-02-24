CREATE DATABASE QLCUAHANG
GO

USE QLCUAHANG
GO

CREATE TABLE tblNhomHang (
    ID_NhomHang VARCHAR(6) PRIMARY KEY,
    TenNhomHang NVARCHAR(255) NOT NULL
);
GO

CREATE TABLE tblHangHoa (
    ID_HangHoa VARCHAR(6) PRIMARY KEY,       -- Mã mặt hàng (tăng lên 10 ký tự theo yêu cầu)
    TenHangHoa NVARCHAR(100) NOT NULL,       -- Tên mặt hàng (giới hạn 100 ký tự theo yêu cầu)
    ID_NhomHang VARCHAR(6),                 -- Mã nhóm hàng (tăng lên 10 ký tự theo yêu cầu)
    MauSac NVARCHAR(50) NULL,                -- Màu sắc
    KichThuoc NVARCHAR(50) NULL,             -- Kích thước
    DacTinhKyThuat NVARCHAR(200) NULL,       -- Đặc tính kỹ thuật
    DonViTinh NVARCHAR(20) NULL,             -- Đơn vị tính
    SoLuong INT NULL,                        -- Số Lượng
    Anh VARBINARY(MAX) NULL,                 -- Ảnh (cho phép NULL)
    GiaBan INT NULL,                         -- Giá bán (VND)
    FOREIGN KEY (ID_NhomHang) REFERENCES tblNhomHang(ID_NhomHang)
);
GO

CREATE TABLE tblLogin (
    ID_Login VARCHAR(6) PRIMARY KEY,  -- Khóa chính
    ID_NhanVien VARCHAR(6) NOT NULL,  -- Khóa ngoại tham chiếu đến bảng tblNhanvien
    Username NVARCHAR(50) NOT NULL UNIQUE,  -- Tên đăng nhập (duy nhất)
    Password NVARCHAR(255) NOT NULL,  -- Mật khẩu (nên mã hóa)
    Role NVARCHAR(50) NOT NULL CHECK (Role IN ('Admin', 'Manager', 'SalesStaff', 'CashierStaff', 'InventoryStaff', 'PurchasingStaff')),  -- Vai trò
    FOREIGN KEY (ID_NhanVien) REFERENCES tblNhanvien(ID_NhanVien)
);

CREATE TABLE tblNhanvien (
    ID_NhanVien VARCHAR(6) PRIMARY KEY,
    HoTen NVARCHAR(255) NOT NULL,
    NgaySinh DATE,
    GioiTinh NVARCHAR(10),
    DiaChi NVARCHAR(255),
    SoDienThoai NVARCHAR(20),
    Email NVARCHAR(255) UNIQUE
);

CREATE TABLE tblKhachhang (
    ID_KhachHang VARCHAR(6) PRIMARY KEY,
    TenKhachHang NVARCHAR(255) NOT NULL,
    DiaChi NVARCHAR(255),
    SoDienThoai NVARCHAR(15),
    Email NVARCHAR(100)
);

CREATE TABLE tblNhacungcap (
    ID_NhaCungCap VARCHAR(6) PRIMARY KEY,
    TenNhaCungCap NVARCHAR(255) NOT NULL,
    DiaChi NVARCHAR(255),
    SoDienThoai NVARCHAR(15),
    Email NVARCHAR(100)
);

CREATE TABLE tblHoadonnhaphang (
    ID_HoaDonNhap VARCHAR(6) PRIMARY KEY,
    NgayNhap DATE NOT NULL,
    TongTien INT NOT NULL,
    GhiChu NVARCHAR(255) NULL,
    ID_NhanVien VARCHAR(6) NOT NULL,
    ID_NhaCungCap VARCHAR(6) NOT NULL,
    FOREIGN KEY (ID_NhanVien) REFERENCES tblNhanvien(ID_NhanVien),
    FOREIGN KEY (ID_NhaCungCap) REFERENCES tblNhacungcap(ID_NhaCungCap)
);

CREATE TABLE tblChitietHoadonnhaphang (
    ID_HoaDonNhap VARCHAR(6) NOT NULL,
    ID_HangHoa VARCHAR(6) NOT NULL,
    SoLuong INT NOT NULL,
    DonGia INT NOT NULL,
    ThanhTien INT NOT NULL,
    CONSTRAINT PK_ChitietHoadonnhaphang PRIMARY KEY (ID_HoaDonNhap, ID_HangHoa),  -- Khóa chính tổ hợp
    FOREIGN KEY (ID_HoaDonNhap) REFERENCES tblHoadonnhaphang(ID_HoaDonNhap),
    FOREIGN KEY (ID_HangHoa) REFERENCES tblHangHoa(ID_HangHoa)
);

CREATE TABLE tblHoadonbanhang (
    ID_HoaDonBan VARCHAR(6) NOT NULL,
    ID_NhanVien VARCHAR(6),
    ID_KhachHang VARCHAR(6),
    NgayBan DATE NOT NULL,
    TongTien INT NOT NULL,
    DaThuTien BIT DEFAULT 0,
    FOREIGN KEY (ID_NhanVien) REFERENCES tblNhanvien(ID_NhanVien),
    FOREIGN KEY (ID_KhachHang) REFERENCES tblKhachhang(ID_KhachHang)
);

CREATE TABLE tblHangban (
    ID_HangBan VARCHAR(6) NOT NULL,
    ID_HangHoa INT,
    ID_HoaDonBan INT,
    SoLuong INT NOT NULL,
    QuiCach NVARCHAR(255),
    GiaBan INT NOT NULL,
    BaoHanh NVARCHAR(255),
    FOREIGN KEY (ID_HangHoa) REFERENCES tblHanghoa(ID_HangHoa),
    FOREIGN KEY (ID_HoaDonBan) REFERENCES tblHoadonbanhang(ID_HoaDonBan)
);



-- STORE PROCEDURE NHOMHANG

CREATE PROCEDURE sp_NhomHang_CRUD
    @Action VARCHAR(10),              -- Tham số xác định hành động: 'INSERT', 'SELECT', 'UPDATE', 'DELETE'
    @ID_NhomHang VARCHAR(6) = NULL,   -- Mã nhóm hàng (NULL khi không cần)
    @TenNhomHang NVARCHAR(255) = NULL -- Tên nhóm hàng (NULL khi không cần)
AS
BEGIN
    SET NOCOUNT ON;

    -- INSERT: Thêm mới nhóm hàng
    IF @Action = 'INSERT'
    BEGIN
        DECLARE @NextID INT;
        DECLARE @NewCode VARCHAR(6);

        -- Tìm mã nhỏ nhất chưa sử dụng
        SET @NextID = 1;
        WHILE EXISTS (SELECT 1 FROM tblNhomHang WHERE ID_NhomHang = 'NMH' + RIGHT('000' + CAST(@NextID AS VARCHAR(3)), 3))
        BEGIN
            SET @NextID = @NextID + 1;
        END;
        SET @NewCode = 'NMH' + RIGHT('000' + CAST(@NextID AS VARCHAR(3)), 3);

        -- Thêm bản ghi mới
        INSERT INTO tblNhomHang (ID_NhomHang, TenNhomHang)
        VALUES (@NewCode, @TenNhomHang);

        SELECT @NewCode AS ID_NhomHang; -- Trả về mã vừa tạo
    END

    -- SELECT: Lấy dữ liệu
    ELSE IF @Action = 'SELECT'
    BEGIN
        IF @ID_NhomHang IS NULL
            SELECT ID_NhomHang, TenNhomHang FROM tblNhomHang;
        ELSE
            SELECT ID_NhomHang, TenNhomHang FROM tblNhomHang WHERE ID_NhomHang = @ID_NhomHang;
    END

    -- UPDATE: Cập nhật nhóm hàng
    ELSE IF @Action = 'UPDATE'
    BEGIN
        UPDATE tblNhomHang
        SET TenNhomHang = @TenNhomHang
        WHERE ID_NhomHang = @ID_NhomHang;

        SELECT @@ROWCOUNT AS RowsAffected; -- Trả về số hàng bị ảnh hưởng
    END

    -- DELETE: Xóa nhóm hàng
    ELSE IF @Action = 'DELETE'
    BEGIN
        DELETE FROM tblNhomHang
        WHERE ID_NhomHang = @ID_NhomHang;

        SELECT @@ROWCOUNT AS RowsAffected; -- Trả về số hàng bị ảnh hưởng
    END
    ELSE
    BEGIN
        RAISERROR ('Invalid Action. Use INSERT, SELECT, UPDATE, or DELETE.', 16, 1);
    END
END;
GO

-- Ví dụ sử dụng:
EXEC sp_NhomHang_CRUD 'INSERT', NULL, N'Nhóm hàng 1'; -- Tạo MH001
EXEC sp_NhomHang_CRUD 'INSERT', NULL, N'Nhóm hàng 2'; -- Tạo MH002
EXEC sp_NhomHang_CRUD 'SELECT', NULL, NULL;          -- Xem tất cả
EXEC sp_NhomHang_CRUD 'DELETE', 'NMH002', NULL;       -- Xóa MH002
EXEC sp_NhomHang_CRUD 'INSERT', NULL, N'Nhóm mới';    -- Tái sử dụng MH002
EXEC sp_NhomHang_CRUD 'UPDATE', 'NMH001', N'Nhóm 1 mới';
EXEC sp_NhomHang_CRUD 'SELECT', 'NMH001', NULL;       -- Xem MH001


-- STORE PROCEDURE HANGHOA  

CREATE PROCEDURE sp_HangHoa_CRUD
    @Action VARCHAR(10),
    @ID_HangHoa VARCHAR(10) = NULL,
    @TenHangHoa NVARCHAR(100) = NULL,
    @ID_NhomHang VARCHAR(10) = NULL,
    @MauSac NVARCHAR(50) = NULL,
    @KichThuoc NVARCHAR(50) = NULL,
    @DacTinhKyThuat NVARCHAR(200) = NULL,
    @DonViTinh NVARCHAR(20) = NULL,
    @SoLuong INT = NULL,
    @GiaBan INT = NULL,
    @Anh VARBINARY(MAX) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF @Action = 'INSERT'
    BEGIN

        -- Kiểm tra ID_NhomHang có tồn tại không
        IF @ID_NhomHang IS NOT NULL AND NOT EXISTS (SELECT 1 FROM tblNhomHang WHERE ID_NhomHang = @ID_NhomHang)
        BEGIN
            RAISERROR ('ID_NhomHang không tồn tại trong tblNhomHang.', 16, 1);
            RETURN;
        END

		DECLARE @NextID INT;
        DECLARE @NewCode VARCHAR(10);

        -- Lấy số lớn nhất từ các mã hiện có dạng HHxxx và tăng lên 1
        SELECT @NextID = ISNULL(MAX(CAST(SUBSTRING(ID_HangHoa, 3, 3) AS INT)), 0) + 1
        FROM tblHangHoa
        WHERE ID_HangHoa LIKE 'HH[0-9][0-9][0-9]';

        -- Nếu bảng rỗng, bắt đầu từ 1
        IF @NextID IS NULL SET @NextID = 1;

        -- Tạo mã mới (HHxxx)
        SET @NewCode = 'HH' + RIGHT('000' + CAST(@NextID AS VARCHAR(3)), 3);

        -- Kiểm tra nếu mã vượt quá HH999
        IF @NextID > 999
        BEGIN
            RAISERROR ('Đã vượt quá số lượng mã tối đa (HH999).', 16, 1);
            RETURN;
        END;

        INSERT INTO tblHangHoa (ID_HangHoa, TenHangHoa, ID_NhomHang, MauSac, KichThuoc, DacTinhKyThuat, DonViTinh, SoLuong, GiaBan, Anh)
        VALUES (@NewCode, @TenHangHoa, @ID_NhomHang, @MauSac, @KichThuoc, @DacTinhKyThuat, @DonViTinh, @SoLuong, @GiaBan, @Anh);

        SELECT @NewCode AS ID_HangHoa;
    END

    ELSE IF @Action = 'SELECT'
    BEGIN
        IF @ID_HangHoa IS NULL
            SELECT 
            hh.ID_HangHoa, 
            hh.TenHangHoa, 
            nh.TenNhomHang,
            hh.MauSac, 
            hh.KichThuoc, 
            hh.DacTinhKyThuat, 
            hh.DonViTinh, 
            hh.SoLuong, 
            hh.GiaBan, 
            hh.Anh
        FROM tblHangHoa hh
        LEFT JOIN tblNhomHang nh ON hh.ID_NhomHang = nh.ID_NhomHang
        WHERE @ID_HangHoa IS NULL OR hh.ID_HangHoa = @ID_HangHoa;
    END

    ELSE IF @Action = 'UPDATE'
    BEGIN
        UPDATE tblHangHoa
        SET TenHangHoa = @TenHangHoa,
            ID_NhomHang = @ID_NhomHang,
            MauSac = @MauSac,
            KichThuoc = @KichThuoc,
            DacTinhKyThuat = @DacTinhKyThuat,
            DonViTinh = @DonViTinh,
            SoLuong = @SoLuong,
            GiaBan = @GiaBan,
            Anh = @Anh
        WHERE ID_HangHoa = @ID_HangHoa;
        SELECT @@ROWCOUNT AS RowsAffected;
    END

    ELSE IF @Action = 'DELETE'
    BEGIN
        DELETE FROM tblHangHoa WHERE ID_HangHoa = @ID_HangHoa;
        SELECT @@ROWCOUNT AS RowsAffected;
    END
    ELSE
    BEGIN
        RAISERROR ('Invalid Action. Use INSERT, SELECT, UPDATE, or DELETE.', 16, 1);
    END
END;
GO

-- Ví dụ sử dụng:
EXEC sp_HangHoa_CRUD 'INSERT', NULL, N'Mặt hàng 1', N'NMH001', N'Đỏ', 'L', N'Chất liệu cotton', N'Cái', 100, 2000000, NULL; -- Tạo HH001
EXEC sp_HangHoa_CRUD 'INSERT', NULL, N'Mặt hàng 2', N'NMH002', N'Xanh', 'M', N'Chống nước', N'Hộp', 50, 1500000, NULL; -- Tạo HH002
EXEC sp_HangHoa_CRUD 'SELECT', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL; -- Xem tất cả
EXEC sp_HangHoa_CRUD 'DELETE', 'HH002', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL; -- Xóa HH002
EXEC sp_HangHoa_CRUD 'INSERT', NULL, N'Mặt hàng mới', 'NMH001', N'Trắng', 'S', N'Nhẹ', N'Chiếc', 200, 3000000, NULL; -- Tái sử dụng HH002
EXEC sp_HangHoa_CRUD 'UPDATE', 'HH001', N'Mặt hàng 1 mới', 'NMH002', N'Vàng', 'XL', N'Chống cháy', N'Bộ', 150, 2500000, NULL;
EXEC sp_HangHoa_CRUD 'SELECT', 'HH001', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL; -- Xem HH001



CREATE PROCEDURE sp_NhanVien_CRUD
    @Action VARCHAR(10),              -- Hành động: 'INSERT', 'SELECT', 'UPDATE', 'DELETE'
    @ID_NhanVien VARCHAR(6) = NULL,   -- Mã nhân viên (NULL khi không cần)
    @HoTen NVARCHAR(255) = NULL,      -- Họ tên
    @NgaySinh DATE = NULL,            -- Ngày sinh
    @GioiTinh NVARCHAR(10) = NULL,    -- Giới tính
    @DiaChi NVARCHAR(255) = NULL,     -- Địa chỉ
    @SoDienThoai NVARCHAR(20) = NULL, -- Số điện thoại
    @Email NVARCHAR(255) = NULL       -- Email
AS
BEGIN
    SET NOCOUNT ON;

    -- INSERT: Thêm mới nhân viên
    IF @Action = 'INSERT'
    BEGIN
        DECLARE @NextID INT;
        DECLARE @NewCode VARCHAR(6);

        -- Tìm mã nhỏ nhất chưa sử dụng
        SET @NextID = 1;
        WHILE EXISTS (SELECT 1 FROM tblNhanVien WHERE ID_NhanVien = 'NV' + RIGHT('000' + CAST(@NextID AS VARCHAR(3)), 3))
        BEGIN
            SET @NextID = @NextID + 1;
        END;
        SET @NewCode = 'NV' + RIGHT('000' + CAST(@NextID AS VARCHAR(3)), 3);

        -- Thêm bản ghi mới
        INSERT INTO tblNhanVien (ID_NhanVien, HoTen, NgaySinh, GioiTinh, DiaChi, SoDienThoai, Email)
        VALUES (@NewCode, @HoTen, @NgaySinh, @GioiTinh, @DiaChi, @SoDienThoai, @Email);

        SELECT @NewCode AS ID_NhanVien; -- Trả về mã vừa tạo
    END

    -- SELECT: Lấy dữ liệu
    ELSE IF @Action = 'SELECT'
    BEGIN
        IF @ID_NhanVien IS NULL
            SELECT ID_NhanVien, HoTen, NgaySinh, GioiTinh, DiaChi, SoDienThoai, Email 
            FROM tblNhanVien;
        ELSE
            SELECT ID_NhanVien, HoTen, NgaySinh, GioiTinh, DiaChi, SoDienThoai, Email 
            FROM tblNhanVien 
            WHERE ID_NhanVien = @ID_NhanVien;
    END

    -- UPDATE: Cập nhật nhân viên
    ELSE IF @Action = 'UPDATE'
    BEGIN
        UPDATE tblNhanVien
        SET HoTen = @HoTen,
            NgaySinh = @NgaySinh,
            GioiTinh = @GioiTinh,
            DiaChi = @DiaChi,
            SoDienThoai = @SoDienThoai,
            Email = @Email
        WHERE ID_NhanVien = @ID_NhanVien;

        SELECT @@ROWCOUNT AS RowsAffected; -- Trả về số hàng bị ảnh hưởng
    END

    -- DELETE: Xóa nhân viên
    ELSE IF @Action = 'DELETE'
    BEGIN
        DELETE FROM tblNhanVien
        WHERE ID_NhanVien = @ID_NhanVien;

        SELECT @@ROWCOUNT AS RowsAffected; -- Trả về số hàng bị ảnh hưởng
    END
    ELSE
    BEGIN
        RAISERROR ('Invalid Action. Use INSERT, SELECT, UPDATE, or DELETE.', 16, 1);
    END
END;
GO

EXEC sp_NhanVien_CRUD 'INSERT', NULL, N'Nguyễn Văn A', '1990-01-01', N'Nam', N'Hà Nội', N'0901234567', N'ava@example.com';
EXEC sp_NhanVien_CRUD 'SELECT';
EXEC sp_NhanVien_CRUD 'UPDATE', 'NV001', N'Nguyễn Văn B', '1990-01-01', N'Nam', N'TP.HCM', N'0909876543', N'nvb@example.com';
EXEC sp_NhanVien_CRUD 'DELETE', 'NV001';


CREATE PROCEDURE sp_Login_CRUD
    @Action VARCHAR(10),              -- Hành động: 'INSERT', 'SELECT', 'UPDATE', 'DELETE'
    @ID_Login VARCHAR(6) = NULL,      -- Mã đăng nhập (NULL khi không cần)
    @ID_NhanVien VARCHAR(6) = NULL,   -- Mã nhân viên
    @Username VARCHAR(50) = NULL,    -- Tên đăng nhập
    @Password VARCHAR(255) = NULL,   -- Mật khẩu
    @Role NVARCHAR(50) = NULL         -- Vai trò
AS
BEGIN
    SET NOCOUNT ON;

    -- INSERT: Thêm mới tài khoản đăng nhập
    IF @Action = 'INSERT'
    BEGIN
        DECLARE @NextID INT;
        DECLARE @NewCode VARCHAR(6);

        -- Tìm mã nhỏ nhất chưa sử dụng
        SET @NextID = 1;
        WHILE EXISTS (SELECT 1 FROM tblLogin WHERE ID_Login = 'LG' + RIGHT('000' + CAST(@NextID AS VARCHAR(3)), 3))
        BEGIN
            SET @NextID = @NextID + 1;
        END;
        SET @NewCode = 'LG' + RIGHT('000' + CAST(@NextID AS VARCHAR(3)), 3);

		-- Mã hóa mật khẩu
        DECLARE @HashedPassword VARBINARY(64);
        SET @HashedPassword = HASHBYTES('SHA2_256', @Password);

        -- Thêm bản ghi mới
        INSERT INTO tblLogin (ID_Login, ID_NhanVien, Username, Password, Role)
        VALUES (@NewCode, @ID_NhanVien, @Username, @HashedPassword, @Role);

        SELECT @NewCode AS ID_Login; -- Trả về mã vừa tạo
    END

    -- SELECT: Lấy dữ liệu
    ELSE IF @Action = 'SELECT'
    BEGIN
        IF @ID_Login IS NULL
            SELECT ID_Login, ID_NhanVien, Username, Password, Role 
            FROM tblLogin;
        ELSE
            SELECT ID_Login, ID_NhanVien, Username, Password, Role 
            FROM tblLogin 
            WHERE ID_Login = @ID_Login;
    END

    -- UPDATE: Cập nhật tài khoản đăng nhập
    ELSE IF @Action = 'UPDATE'
    BEGIN
        -- Nếu có mật khẩu mới, mã hóa lại
        IF @Password IS NOT NULL
        BEGIN
            DECLARE @UpdatedPassword VARBINARY(64);
            SET @UpdatedPassword = HASHBYTES('SHA2_256', @Password);
            
            UPDATE tblLogin
            SET ID_NhanVien = @ID_NhanVien,
                Username = @Username,
                Password = @UpdatedPassword,
                Role = @Role
            WHERE ID_Login = @ID_Login;
        END
        ELSE
        BEGIN
            UPDATE tblLogin
            SET ID_NhanVien = @ID_NhanVien,
                Username = @Username,
                Role = @Role
            WHERE ID_Login = @ID_Login;
        END

        SELECT @@ROWCOUNT AS RowsAffected; -- Trả về số hàng bị ảnh hưởng
    END

    -- DELETE: Xóa tài khoản đăng nhập
    ELSE IF @Action = 'DELETE'
    BEGIN
        DELETE FROM tblLogin
        WHERE ID_Login = @ID_Login;

        SELECT @@ROWCOUNT AS RowsAffected; -- Trả về số hàng bị ảnh hưởng
    END
    ELSE
    BEGIN
        RAISERROR ('Invalid Action. Use INSERT, SELECT, UPDATE, or DELETE.', 16, 1);
    END
END;
GO

EXEC sp_Login_CRUD 'INSERT', NULL, 'NV002', 'user2', 'password123', N'Admin';
EXEC sp_Login_CRUD 'INSERT', NULL, 'NV001', 'user1', 'adbfhe@in2fk', N'Admin';
EXEC sp_Login_CRUD 'SELECT';
EXEC sp_Login_CRUD 'UPDATE', 'LG001', 'NV0002', 'user2', 'newpass456', N'Manager';
EXEC sp_Login_CRUD 'DELETE', 'LG001';


CREATE PROCEDURE sp_Login
    @Username VARCHAR(50),
    @Password VARCHAR(255),
    @Result INT OUTPUT,          -- 1: Thành công, 0: Thất bại
    @Role NVARCHAR(50) OUTPUT,
    @ID_NhanVien VARCHAR(6) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Mã hóa mật khẩu đầu vào
    DECLARE @HashedPassword VARBINARY(64);
    SET @HashedPassword = HASHBYTES('SHA2_256', @Password);
    
    -- Kiểm tra thông tin đăng nhập với mật khẩu đã mã hóa
    IF EXISTS (
        SELECT 1 
        FROM tblLogin 
        WHERE Username = @Username 
        AND Password = @HashedPassword  -- So sánh với mật khẩu đã mã hóa trong DB
    )
    BEGIN
        -- Lấy thông tin người dùng
        SELECT 
            @Role = Role,
            @ID_NhanVien = ID_NhanVien
        FROM tblLogin
        WHERE Username = @Username
        AND Password = @HashedPassword;
        
        SET @Result = 1;  -- Đăng nhập thành công
    END
    ELSE
    BEGIN
        SET @Result = 0;      -- Đăng nhập thất bại
        SET @Role = NULL;
        SET @ID_NhanVien = NULL;
    END
END
GO

-- Stored Procedure cho Đăng Xuất (giữ nguyên)
CREATE PROCEDURE sp_Logout
    @Username NVARCHAR(50),
    @Result INT OUTPUT          -- 1: Thành công, 0: Thất bại
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Kiểm tra username có tồn tại không
    IF EXISTS (
        SELECT 1 
        FROM tblLogin 
        WHERE Username = @Username
    )
    BEGIN
        -- Ở đây có thể thêm logic ghi log đăng xuất nếu cần
        SET @Result = 1;  -- Đăng xuất thành công
    END
    ELSE
    BEGIN
        SET @Result = 0;  -- Đăng xuất thất bại
    END
END
GO

DECLARE @Result INT, @Role NVARCHAR(50), @ID_NhanVien VARCHAR(6);
EXEC sp_Login 
    @Username = 'user2',
    @Password = 'password123',  -- Mật khẩu plain text sẽ được mã hóa trong SP
    @Result = @Result OUTPUT,
    @Role = @Role OUTPUT,
    @ID_NhanVien = @ID_NhanVien OUTPUT;

IF @Result = 1
    PRINT N'Đăng nhập thành công! Role: ' + @Role + N', ID Nhân viên: ' + @ID_NhanVien;
ELSE
    PRINT N'Đăng nhập thất bại!';
