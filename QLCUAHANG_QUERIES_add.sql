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

CREATE TABLE tblNhanvien (
    ID_NhanVien VARCHAR(6) PRIMARY KEY,
    HoTen NVARCHAR(255) NOT NULL,
    NgaySinh DATE,
    GioiTinh NVARCHAR(10),
    DiaChi NVARCHAR(255),
    SoDienThoai NVARCHAR(20),
    Email NVARCHAR(255) UNIQUE
);

CREATE TABLE tblLogin (
    ID_Login VARCHAR(6) PRIMARY KEY,  -- Khóa chính
    ID_NhanVien VARCHAR(6) NOT NULL,  -- Khóa ngoại tham chiếu đến bảng tblNhanvien
    Username NVARCHAR(50) NOT NULL UNIQUE,  -- Tên đăng nhập (duy nhất)
    Password NVARCHAR(255) NOT NULL,  -- Mật khẩu (nên mã hóa)
    Role NVARCHAR(50) NOT NULL CHECK (Role IN ('Admin', 'Manager', 'SalesStaff', 'CashierStaff', 'InventoryStaff', 'PurchasingStaff')),  -- Vai trò
    FOREIGN KEY (ID_NhanVien) REFERENCES tblNhanvien(ID_NhanVien)
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
    ID_HoaDonBan VARCHAR(6) PRIMARY KEY,
    ID_NhanVien VARCHAR(6),
    ID_KhachHang VARCHAR(6),
    NgayBan DATE NOT NULL,
    TongTien INT NOT NULL,
    DaThuTien BIT DEFAULT 0,
    FOREIGN KEY (ID_NhanVien) REFERENCES tblNhanvien(ID_NhanVien),
    FOREIGN KEY (ID_KhachHang) REFERENCES tblKhachhang(ID_KhachHang)
);

CREATE TABLE tblHangban (
    ID_HangHoa VARCHAR(6) NOT NULL,
    ID_HoaDonBan VARCHAR(6) NOT NULL,
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

		  ELSE IF @Action = 'SELECT_ONE'
	BEGIN
		IF @ID_HangHoa IS NOT NULL
		BEGIN
			SELECT 
				hh.ID_HangHoa, 
				hh.SoLuong
			FROM tblHangHoa hh
			LEFT JOIN tblNhomHang nh ON hh.ID_NhomHang = nh.ID_NhomHang
			WHERE hh.ID_HangHoa = @ID_HangHoa;
		END
		ELSE
		BEGIN
			SELECT 
				hh.ID_HangHoa, 
				hh.SoLuong
			FROM tblHangHoa hh
			LEFT JOIN tblNhomHang nh ON hh.ID_NhomHang = nh.ID_NhomHang;
		END
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



CREATE PROCEDURE sp_NhaCungCap_CRUD
    @Action VARCHAR(10),              -- Hành động: 'INSERT', 'SELECT', 'UPDATE', 'DELETE'
    @ID_NhaCungCap VARCHAR(6) = NULL, -- Mã nhà cung cấp (NULL khi không cần)
    @TenNhaCungCap NVARCHAR(255) = NULL, -- Tên nhà cung cấp
    @DiaChi NVARCHAR(255) = NULL,     -- Địa chỉ
    @SoDienThoai NVARCHAR(15) = NULL, -- Số điện thoại
    @Email NVARCHAR(100) = NULL       -- Email
AS
BEGIN
    SET NOCOUNT ON;

    -- INSERT: Thêm mới nhà cung cấp
    IF @Action = 'INSERT'
    BEGIN
        DECLARE @NextID INT;
        DECLARE @NewCode VARCHAR(6);

        -- Tìm mã nhỏ nhất chưa sử dụng
        SET @NextID = 1;
        WHILE EXISTS (SELECT 1 FROM tblNhacungcap WHERE ID_NhaCungCap = 'NCC' + RIGHT('000' + CAST(@NextID AS VARCHAR(3)), 3))
        BEGIN
            SET @NextID = @NextID + 1;
        END;
        SET @NewCode = 'NCC' + RIGHT('000' + CAST(@NextID AS VARCHAR(3)), 3);

        -- Thêm bản ghi mới
        INSERT INTO tblNhacungcap (ID_NhaCungCap, TenNhaCungCap, DiaChi, SoDienThoai, Email)
        VALUES (@NewCode, @TenNhaCungCap, @DiaChi, @SoDienThoai, @Email);

        SELECT @NewCode AS ID_NhaCungCap; -- Trả về mã vừa tạo
    END

    -- SELECT: Lấy dữ liệu
    ELSE IF @Action = 'SELECT'
    BEGIN
        IF @ID_NhaCungCap IS NULL
            SELECT ID_NhaCungCap, TenNhaCungCap, DiaChi, SoDienThoai, Email 
            FROM tblNhacungcap;
        ELSE
            SELECT ID_NhaCungCap, TenNhaCungCap, DiaChi, SoDienThoai, Email 
            FROM tblNhacungcap 
            WHERE ID_NhaCungCap = @ID_NhaCungCap;
    END

    -- UPDATE: Cập nhật nhà cung cấp
    ELSE IF @Action = 'UPDATE'
    BEGIN
        UPDATE tblNhacungcap
        SET TenNhaCungCap = @TenNhaCungCap,
            DiaChi = @DiaChi,
            SoDienThoai = @SoDienThoai,
            Email = @Email
        WHERE ID_NhaCungCap = @ID_NhaCungCap;

        SELECT @@ROWCOUNT AS RowsAffected; -- Trả về số hàng bị ảnh hưởng
    END

    -- DELETE: Xóa nhà cung cấp
    ELSE IF @Action = 'DELETE'
    BEGIN
        DELETE FROM tblNhacungcap
        WHERE ID_NhaCungCap = @ID_NhaCungCap;

        SELECT @@ROWCOUNT AS RowsAffected; -- Trả về số hàng bị ảnh hưởng
    END
    ELSE
    BEGIN
        RAISERROR ('Invalid Action. Use INSERT, SELECT, UPDATE, or DELETE.', 16, 1);
    END
END;
GO

CREATE PROCEDURE sp_KhachHang_CRUD
    @Action VARCHAR(10),              -- Hành động: 'INSERT', 'SELECT', 'UPDATE', 'DELETE'
    @ID_KhachHang VARCHAR(6) = NULL,  -- Mã khách hàng (NULL khi không cần)
    @TenKhachHang NVARCHAR(255) = NULL, -- Tên khách hàng
    @DiaChi NVARCHAR(255) = NULL,     -- Địa chỉ
    @SoDienThoai NVARCHAR(15) = NULL, -- Số điện thoại
    @Email NVARCHAR(100) = NULL       -- Email
AS
BEGIN
    SET NOCOUNT ON;

    -- INSERT: Thêm mới khách hàng
    IF @Action = 'INSERT'
    BEGIN
        DECLARE @NextID INT;
        DECLARE @NewCode VARCHAR(6);

        -- Tìm mã nhỏ nhất chưa sử dụng
        SET @NextID = 1;
        WHILE EXISTS (SELECT 1 FROM tblKhachhang WHERE ID_KhachHang = 'KH' + RIGHT('000' + CAST(@NextID AS VARCHAR(3)), 3))
        BEGIN
            SET @NextID = @NextID + 1;
        END;
        SET @NewCode = 'KH' + RIGHT('000' + CAST(@NextID AS VARCHAR(3)), 3);

        -- Thêm bản ghi mới
        INSERT INTO tblKhachhang (ID_KhachHang, TenKhachHang, DiaChi, SoDienThoai, Email)
        VALUES (@NewCode, @TenKhachHang, @DiaChi, @SoDienThoai, @Email);

        SELECT @NewCode AS ID_KhachHang; -- Trả về mã vừa tạo
    END

    -- SELECT: Lấy dữ liệu khách hàng
    ELSE IF @Action = 'SELECT'
    BEGIN
        IF @ID_KhachHang IS NULL
            SELECT ID_KhachHang, TenKhachHang, DiaChi, SoDienThoai, Email 
            FROM tblKhachhang;
        ELSE
            SELECT ID_KhachHang, TenKhachHang, DiaChi, SoDienThoai, Email 
            FROM tblKhachhang 
            WHERE ID_KhachHang = @ID_KhachHang;
    END

    -- UPDATE: Cập nhật khách hàng
    ELSE IF @Action = 'UPDATE'
    BEGIN
        UPDATE tblKhachhang
        SET TenKhachHang = @TenKhachHang,
            DiaChi = @DiaChi,
            SoDienThoai = @SoDienThoai,
            Email = @Email
        WHERE ID_KhachHang = @ID_KhachHang;

        SELECT @@ROWCOUNT AS RowsAffected; -- Trả về số hàng bị ảnh hưởng
    END

    -- DELETE: Xóa khách hàng
    ELSE IF @Action = 'DELETE'
    BEGIN
        DELETE FROM tblKhachhang
        WHERE ID_KhachHang = @ID_KhachHang;

        SELECT @@ROWCOUNT AS RowsAffected; -- Trả về số hàng bị ảnh hưởng
    END
    ELSE
    BEGIN
        RAISERROR ('Invalid Action. Use INSERT, SELECT, UPDATE, or DELETE.', 16, 1);
    END
END;
GO

CREATE PROCEDURE sp_HoaDonNhap_CRUD
    @Action VARCHAR(20),                  -- Hành động: 'INSERT', 'SELECT', 'UPDATE', 'DELETE', 'INSERT_DETAIL', 'UPDATE_DETAIL', 'DELETE_DETAIL'
    @ID_HoaDonNhap VARCHAR(6) = NULL,     -- Mã hóa đơn nhập (NULL khi không cần)
    @NgayNhap DATE = NULL,                -- Ngày nhập
    @TongTien INT = NULL,                 -- Tổng tiền
    @GhiChu NVARCHAR(255) = NULL,         -- Ghi chú
    @ID_NhanVien VARCHAR(6) = NULL,       -- Mã nhân viên
    @ID_NhaCungCap VARCHAR(6) = NULL,     -- Mã nhà cung cấp
    @ID_HangHoa VARCHAR(6) = NULL,        -- Mã hàng hóa (cho chi tiết hóa đơn)
    @SoLuong INT = NULL,                  -- Số lượng (cho chi tiết hóa đơn)
    @DonGia INT = NULL                    -- Đơn giá (cho chi tiết hóa đơn)
AS
BEGIN
    SET NOCOUNT ON;

    -- INSERT: Thêm mới hóa đơn nhập hàng
    IF @Action = 'INSERT'
    BEGIN
        DECLARE @NextID INT;
        DECLARE @NewCode VARCHAR(6);

        -- Tìm mã nhỏ nhất chưa sử dụng
        SET @NextID = 1;
        WHILE EXISTS (SELECT 1 FROM tblHoadonnhaphang WHERE ID_HoaDonNhap = 'HDN' + RIGHT('000' + CAST(@NextID AS VARCHAR(3)), 3))
        BEGIN
            SET @NextID = @NextID + 1;
        END;
        SET @NewCode = 'HDN' + RIGHT('000' + CAST(@NextID AS VARCHAR(3)), 3);

        -- Thêm bản ghi hóa đơn nhập
        INSERT INTO tblHoadonnhaphang (ID_HoaDonNhap, NgayNhap, TongTien, GhiChu, ID_NhanVien, ID_NhaCungCap)
        VALUES (@NewCode, @NgayNhap, @TongTien, @GhiChu, @ID_NhanVien, @ID_NhaCungCap);

        SELECT @NewCode AS ID_HoaDonNhap; -- Trả về mã vừa tạo
    END

    -- SELECT: Lấy dữ liệu hóa đơn nhập và chi tiết
		ELSE IF @Action = 'SELECT'
		BEGIN
			IF @ID_HoaDonNhap IS NULL
				SELECT 
					h.ID_HoaDonNhap, 
					h.NgayNhap, 
					h.TongTien, 
					h.GhiChu, 
					h.ID_NhanVien, 
					nv.HoTen,      -- Tên nhân viên
					h.ID_NhaCungCap, 
					ncc.TenNhaCungCap,  -- Tên nhà cung cấp
					c.ID_HangHoa, 
					c.SoLuong, 
					c.DonGia, 
					c.ThanhTien
				FROM tblHoadonnhaphang h
				LEFT JOIN tblNhanVien nv ON h.ID_NhanVien = nv.ID_NhanVien         -- JOIN với bảng nhân viên
				LEFT JOIN tblNhaCungCap ncc ON h.ID_NhaCungCap = ncc.ID_NhaCungCap -- JOIN với bảng nhà cung cấp
				LEFT JOIN tblChitietHoadonnhaphang c ON h.ID_HoaDonNhap = c.ID_HoaDonNhap;
			ELSE
				SELECT 
					h.ID_HoaDonNhap, 
					h.NgayNhap, 
					h.TongTien, 
					h.GhiChu, 
					h.ID_NhanVien, 
					nv.HoTen,      -- Tên nhân viên
					h.ID_NhaCungCap, 
					ncc.TenNhaCungCap,  -- Tên nhà cung cấp
					c.ID_HangHoa, 
					c.SoLuong, 
					c.DonGia, 
					c.ThanhTien
				FROM tblHoadonnhaphang h
				LEFT JOIN tblNhanVien nv ON h.ID_NhanVien = nv.ID_NhanVien         -- JOIN với bảng nhân viên
				LEFT JOIN tblNhaCungCap ncc ON h.ID_NhaCungCap = ncc.ID_NhaCungCap -- JOIN với bảng nhà cung cấp
				LEFT JOIN tblChitietHoadonnhaphang c ON h.ID_HoaDonNhap = c.ID_HoaDonNhap
				WHERE h.ID_HoaDonNhap = @ID_HoaDonNhap;
		END

		ELSE IF @Action = 'SELECT_DETAIL'
	BEGIN
		IF @ID_HoaDonNhap IS NULL
		BEGIN
			RAISERROR ('ID_HoaDonNhap cannot be NULL for SELECT_DETAIL action.', 16, 1);
			RETURN;
		END

		SELECT 
			c.ID_HoaDonNhap,
			c.ID_HangHoa,
			hh.TenHangHoa,  -- Tên hàng hóa từ tblHangHoa
			c.SoLuong,
			c.DonGia,
			c.ThanhTien
		FROM tblChitietHoadonnhaphang c
		INNER JOIN tblHangHoa hh ON c.ID_HangHoa = hh.ID_HangHoa  -- JOIN với tblHangHoa để lấy TenHangHoa
		WHERE c.ID_HoaDonNhap = @ID_HoaDonNhap;
	END

    -- UPDATE: Cập nhật hóa đơn nhập hàng
    ELSE IF @Action = 'UPDATE'
    BEGIN
        UPDATE tblHoadonnhaphang
        SET NgayNhap = @NgayNhap,
            TongTien = @TongTien,
            GhiChu = @GhiChu,
            ID_NhanVien = @ID_NhanVien,
            ID_NhaCungCap = @ID_NhaCungCap
        WHERE ID_HoaDonNhap = @ID_HoaDonNhap;

        SELECT @@ROWCOUNT AS RowsAffected; -- Trả về số hàng bị ảnh hưởng
    END

    -- DELETE: Xóa hóa đơn nhập hàng (xóa cả chi tiết)
    ELSE IF @Action = 'DELETE'
    BEGIN
        BEGIN TRANSACTION;
        DELETE FROM tblChitietHoadonnhaphang WHERE ID_HoaDonNhap = @ID_HoaDonNhap;
        DELETE FROM tblHoadonnhaphang WHERE ID_HoaDonNhap = @ID_HoaDonNhap;
        COMMIT TRANSACTION;

        SELECT @@ROWCOUNT AS RowsAffected; -- Trả về số hàng bị ảnh hưởng từ tblHoadonnhaphang
    END

    -- INSERT_DETAIL: Thêm chi tiết hóa đơn nhập hàng
    ELSE IF @Action = 'INSERT_DETAIL'
    BEGIN
        DECLARE @ThanhTien BIGINT = @SoLuong * @DonGia;

        INSERT INTO tblChitietHoadonnhaphang (ID_HoaDonNhap, ID_HangHoa, SoLuong, DonGia, ThanhTien)
        VALUES (@ID_HoaDonNhap, @ID_HangHoa, @SoLuong, @DonGia, @ThanhTien);

		-- Cập nhật số lượng và giá bán trong tblHangHoa
        UPDATE tblHangHoa
        SET SoLuong = CASE WHEN SoLuong IS NULL THEN @SoLuong ELSE SoLuong + @SoLuong END,
            GiaBan = @DonGia
        WHERE ID_HangHoa = @ID_HangHoa;

        -- Cập nhật tổng tiền trong tblHoadonnhaphang
        UPDATE tblHoadonnhaphang
        SET TongTien = (SELECT SUM(ThanhTien) FROM tblChitietHoadonnhaphang WHERE ID_HoaDonNhap = @ID_HoaDonNhap)
        WHERE ID_HoaDonNhap = @ID_HoaDonNhap;

        SELECT @@ROWCOUNT AS RowsAffected; -- Trả về số hàng bị ảnh hưởng từ tblHoadonnhaphang
    END

    -- UPDATE_DETAIL: Cập nhật chi tiết hóa đơn nhập hàng
    ELSE IF @Action = 'UPDATE_DETAIL'
    BEGIN
       DECLARE @OldSoLuong INT;
        DECLARE @NewThanhTien BIGINT = @SoLuong * @DonGia;

        -- Lấy số lượng cũ từ chi tiết hóa đơn
        SELECT @OldSoLuong = SoLuong
        FROM tblChitietHoadonnhaphang
        WHERE ID_HoaDonNhap = @ID_HoaDonNhap AND ID_HangHoa = @ID_HangHoa;

        -- Cập nhật chi tiết hóa đơn
        UPDATE tblChitietHoadonnhaphang
        SET SoLuong = @SoLuong,
            DonGia = @DonGia,
            ThanhTien = @NewThanhTien
        WHERE ID_HoaDonNhap = @ID_HoaDonNhap AND ID_HangHoa = @ID_HangHoa;

        -- Cập nhật số lượng và giá bán trong tblHangHoa
        UPDATE tblHangHoa
        SET SoLuong = ISNULL(SoLuong, 0) - ISNULL(@OldSoLuong, 0) + @SoLuong, -- Điều chỉnh dựa trên sự thay đổi
            GiaBan = @DonGia
        WHERE ID_HangHoa = @ID_HangHoa;

        -- Cập nhật tổng tiền trong tblHoadonnhaphang
        UPDATE tblHoadonnhaphang
        SET TongTien = (SELECT SUM(ThanhTien) FROM tblChitietHoadonnhaphang WHERE ID_HoaDonNhap = @ID_HoaDonNhap)
        WHERE ID_HoaDonNhap = @ID_HoaDonNhap;

        SELECT @@ROWCOUNT AS RowsAffected; -- Trả về số hàng bị ảnh hưởng từ tblHoadonnhaphang
    END

    -- DELETE_DETAIL: Xóa chi tiết hóa đơn nhập hàng
    ELSE IF @Action = 'DELETE_DETAIL'
    BEGIN
        DECLARE @DeletedSoLuong INT;

        -- Lấy số lượng trước khi xóa
        SELECT @DeletedSoLuong = SoLuong
        FROM tblChitietHoadonnhaphang
        WHERE ID_HoaDonNhap = @ID_HoaDonNhap AND ID_HangHoa = @ID_HangHoa;

        -- Xóa chi tiết hóa đơn
        DELETE FROM tblChitietHoadonnhaphang
        WHERE ID_HoaDonNhap = @ID_HoaDonNhap AND ID_HangHoa = @ID_HangHoa;

        -- Cập nhật số lượng trong tblHangHoa
        UPDATE tblHangHoa
        SET SoLuong = SoLuong - @DeletedSoLuong
        WHERE ID_HangHoa = @ID_HangHoa;

        -- Cập nhật tổng tiền trong tblHoadonnhaphang
        UPDATE tblHoadonnhaphang
        SET TongTien = ISNULL((SELECT SUM(ThanhTien) FROM tblChitietHoadonnhaphang WHERE ID_HoaDonNhap = @ID_HoaDonNhap), 0)
        WHERE ID_HoaDonNhap = @ID_HoaDonNhap;

        SELECT @@ROWCOUNT AS RowsAffected;
    END
    ELSE
    BEGIN
        RAISERROR ('Invalid Action. Use INSERT, SELECT, UPDATE, DELETE, INSERT_DETAIL, UPDATE_DETAIL, or DELETE_DETAIL.', 16, 1);
    END

END;
GO

CREATE PROCEDURE sp_HoaDon_CRUD
    @Action VARCHAR(20),
    @ID_HoaDonBan VARCHAR(6) = NULL,
    @ID_NhanVien VARCHAR(6) = NULL,
    @ID_KhachHang VARCHAR(6) = NULL,
    @NgayBan DATE = NULL,
    @TongTien INT = NULL,
    @DaThuTien BIT = NULL,
    @ID_HangHoa VARCHAR(6) = NULL,
    @SoLuong INT = NULL,
    @QuiCach NVARCHAR(255) = NULL,
    @GiaBan INT = NULL,
    @BaoHanh NVARCHAR(255) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    -- INSERT: Thêm mới hóa đơn bán hàng
    IF @Action = 'INSERT'
    BEGIN
        DECLARE @NextID INT;
        DECLARE @NewCode VARCHAR(6);

        SET @NextID = 1;
        WHILE EXISTS (SELECT 1 FROM tblHoadonbanhang WHERE ID_HoaDonBan = 'HDB' + RIGHT('000' + CAST(@NextID AS VARCHAR(3)), 3))
        BEGIN
            SET @NextID = @NextID + 1;
        END;
        SET @NewCode = 'HDB' + RIGHT('000' + CAST(@NextID AS VARCHAR(3)), 3);

        INSERT INTO tblHoadonbanhang (ID_HoaDonBan, ID_NhanVien, ID_KhachHang, NgayBan, TongTien, DaThuTien)
        VALUES (@NewCode, @ID_NhanVien, @ID_KhachHang, @NgayBan, @TongTien, @DaThuTien);

        SELECT @NewCode AS ID_HoaDonBan;
    END

    -- SELECT: Lấy dữ liệu hóa đơn bán và chi tiết
		ELSE IF @Action = 'SELECT'
	BEGIN
		IF @ID_HoaDonBan IS NULL
			SELECT 
				h.ID_HoaDonBan,
				h.ID_NhanVien,
				nv.HoTen, -- Thêm tên nhân viên
				h.ID_KhachHang,
				kh.TenKhachHang, -- Thêm tên khách hàng
				h.NgayBan,
				h.TongTien,
				h.DaThuTien,
				c.ID_HangHoa,
				hh.TenHangHoa, -- Thêm tên hàng hóa
				c.SoLuong,
				c.QuiCach,
				c.GiaBan,
				c.BaoHanh
			FROM tblHoadonbanhang h
			LEFT JOIN tblHangban c ON h.ID_HoaDonBan = c.ID_HoaDonBan
			LEFT JOIN tblNhanVien nv ON h.ID_NhanVien = nv.ID_NhanVien
			LEFT JOIN tblKhachHang kh ON h.ID_KhachHang = kh.ID_KhachHang
			LEFT JOIN tblHangHoa hh ON c.ID_HangHoa = hh.ID_HangHoa;
		ELSE
			SELECT 
				h.ID_HoaDonBan,
				h.ID_NhanVien,
				nv.HoTen, -- Thêm tên nhân viên
				h.ID_KhachHang,
				kh.TenKhachHang, -- Thêm tên khách hàng
				h.NgayBan,
				h.TongTien,
				h.DaThuTien,
				c.ID_HangHoa,
				hh.TenHangHoa, -- Thêm tên hàng hóa
				c.SoLuong,
				c.QuiCach,
				c.GiaBan,
				c.BaoHanh
			FROM tblHoadonbanhang h
			LEFT JOIN tblHangban c ON h.ID_HoaDonBan = c.ID_HoaDonBan
			LEFT JOIN tblNhanVien nv ON h.ID_NhanVien = nv.ID_NhanVien
			LEFT JOIN tblKhachHang kh ON h.ID_KhachHang = kh.ID_KhachHang
			LEFT JOIN tblHangHoa hh ON c.ID_HangHoa = hh.ID_HangHoa
			WHERE h.ID_HoaDonBan = @ID_HoaDonBan;
	END

		ELSE IF @Action = 'SELECT_HOADON_ONLY'
	BEGIN
		IF @ID_HoaDonBan IS NULL
			SELECT 
				ID_HoaDonBan,
				ID_NhanVien,
				ID_KhachHang,
				NgayBan,
				TongTien,
				DaThuTien
			FROM tblHoadonbanhang;
		ELSE
			SELECT 
				ID_HoaDonBan,
				ID_NhanVien,
				ID_KhachHang,
				NgayBan,
				TongTien,
				DaThuTien
			FROM tblHoadonbanhang
			WHERE ID_HoaDonBan = @ID_HoaDonBan;
	END

    -- UPDATE: Cập nhật hóa đơn bán hàng (giữ giá trị cũ nếu không cung cấp)
    ELSE IF @Action = 'UPDATE'
    BEGIN
        DECLARE @OldDaThuTien BIT;

        -- Lấy trạng thái cũ của DaThuTien
        SELECT @OldDaThuTien = DaThuTien
        FROM tblHoadonbanhang
        WHERE ID_HoaDonBan = @ID_HoaDonBan;

        -- Cập nhật chỉ các cột được cung cấp, giữ nguyên các cột khác
        UPDATE tblHoadonbanhang
        SET ID_NhanVien = COALESCE(@ID_NhanVien, ID_NhanVien),
            ID_KhachHang = COALESCE(@ID_KhachHang, ID_KhachHang),
            NgayBan = COALESCE(@NgayBan, NgayBan),
            TongTien = COALESCE(@TongTien, TongTien),
            DaThuTien = COALESCE(@DaThuTien, DaThuTien)
        WHERE ID_HoaDonBan = @ID_HoaDonBan;

        -- Nếu chuyển từ chưa thu tiền (0) sang đã thu tiền (1), trừ kho
        IF @OldDaThuTien = 0 AND COALESCE(@DaThuTien, @OldDaThuTien) = 1
        BEGIN
            UPDATE hh
            SET hh.SoLuong = hh.SoLuong - hb.SoLuong
            FROM tblHanghoa hh
            INNER JOIN tblHangban hb ON hh.ID_HangHoa = hb.ID_HangHoa
            WHERE hb.ID_HoaDonBan = @ID_HoaDonBan;
        END

        -- Nếu chuyển từ đã thu tiền (1) sang chưa thu tiền (0), hoàn kho
        IF @OldDaThuTien = 1 AND COALESCE(@DaThuTien, @OldDaThuTien) = 0
        BEGIN
            UPDATE hh
            SET hh.SoLuong = hh.SoLuong + hb.SoLuong
            FROM tblHanghoa hh
            INNER JOIN tblHangban hb ON hh.ID_HangHoa = hb.ID_HangHoa
            WHERE hb.ID_HoaDonBan = @ID_HoaDonBan;
        END

        SELECT @@ROWCOUNT AS RowsAffected;
    END

    -- DELETE: Xóa hóa đơn bán hàng
    ELSE IF @Action = 'DELETE'
    BEGIN
        DECLARE @WasPaid BIT;

        SELECT @WasPaid = DaThuTien
        FROM tblHoadonbanhang
        WHERE ID_HoaDonBan = @ID_HoaDonBan;

        BEGIN TRANSACTION;
        IF @WasPaid = 1
        BEGIN
            UPDATE hh
            SET hh.SoLuong = hh.SoLuong + hb.SoLuong
            FROM tblHanghoa hh
            INNER JOIN tblHangban hb ON hh.ID_HangHoa = hb.ID_HangHoa
            WHERE hb.ID_HoaDonBan = @ID_HoaDonBan;
        END

        DELETE FROM tblHangban WHERE ID_HoaDonBan = @ID_HoaDonBan;
        DELETE FROM tblHoadonbanhang WHERE ID_HoaDonBan = @ID_HoaDonBan;
        COMMIT TRANSACTION;

        SELECT @@ROWCOUNT AS RowsAffected;
    END

    -- INSERT_DETAIL: Thêm chi tiết hàng bán
    ELSE IF @Action = 'INSERT_DETAIL'
    BEGIN
        INSERT INTO tblHangban (ID_HoaDonBan, ID_HangHoa, SoLuong, QuiCach, GiaBan, BaoHanh)
        VALUES (@ID_HoaDonBan, @ID_HangHoa, @SoLuong, @QuiCach, @GiaBan, @BaoHanh);

        UPDATE tblHoadonbanhang
        SET TongTien = (SELECT SUM(SoLuong * GiaBan) FROM tblHangban WHERE ID_HoaDonBan = @ID_HoaDonBan)
        WHERE ID_HoaDonBan = @ID_HoaDonBan;

        SELECT @@ROWCOUNT AS RowsAffected;
    END

    -- UPDATE_DETAIL: Cập nhật chi tiết hàng bán
    ELSE IF @Action = 'UPDATE_DETAIL'
    BEGIN
        UPDATE tblHangban
        SET SoLuong = @SoLuong,
            QuiCach = @QuiCach,
            GiaBan = @GiaBan,
            BaoHanh = @BaoHanh
        WHERE ID_HoaDonBan = @ID_HoaDonBan AND ID_HangHoa = @ID_HangHoa;

        UPDATE tblHoadonbanhang
        SET TongTien = (SELECT SUM(SoLuong * GiaBan) FROM tblHangban WHERE ID_HoaDonBan = @ID_HoaDonBan)
        WHERE ID_HoaDonBan = @ID_HoaDonBan;

        SELECT @@ROWCOUNT AS RowsAffected;
    END

    -- DELETE_DETAIL: Xóa chi tiết hàng bán
    ELSE IF @Action = 'DELETE_DETAIL'
    BEGIN
        DECLARE @IsPaid BIT;

        SELECT @IsPaid = DaThuTien
        FROM tblHoadonbanhang
        WHERE ID_HoaDonBan = @ID_HoaDonBan;

        IF @IsPaid = 1
        BEGIN
            DECLARE @DeletedSoLuong INT;
            SELECT @DeletedSoLuong = SoLuong
            FROM tblHangban
            WHERE ID_HoaDonBan = @ID_HoaDonBan AND ID_HangHoa = @ID_HangHoa;

            UPDATE tblHanghoa
            SET SoLuong = SoLuong + @DeletedSoLuong
            WHERE ID_HangHoa = @ID_HangHoa;
        END

        DELETE FROM tblHangban
        WHERE ID_HoaDonBan = @ID_HoaDonBan AND ID_HangHoa = @ID_HangHoa;

        UPDATE tblHoadonbanhang
        SET TongTien = ISNULL((SELECT SUM(SoLuong * GiaBan) FROM tblHangban WHERE ID_HoaDonBan = @ID_HoaDonBan), 0)
        WHERE ID_HoaDonBan = @ID_HoaDonBan;

        SELECT @@ROWCOUNT AS RowsAffected;
    END
    ELSE
    BEGIN
        RAISERROR ('Invalid Action. Use INSERT, SELECT, UPDATE, DELETE, INSERT_DETAIL, UPDATE_DETAIL, or DELETE_DETAIL.', 16, 1);
    END
END;
GO

-- Ví dụ sử dụng:
EXEC sp_NhomHang_CRUD 'INSERT', NULL, N'Nhóm hàng 1'; -- Tạo MH001
EXEC sp_NhomHang_CRUD 'INSERT', NULL, N'Nhóm hàng 2'; -- Tạo MH002
EXEC sp_NhomHang_CRUD 'INSERT', NULL, N'Nhóm hàng 3'; -- Tạo MH003
EXEC sp_NhomHang_CRUD 'SELECT', NULL, NULL;          -- Xem tất cả
EXEC sp_NhomHang_CRUD 'DELETE', 'NMH002', NULL;       -- Xóa MH002
EXEC sp_NhomHang_CRUD 'INSERT', NULL, N'Nhóm mới';    -- Tái sử dụng MH002
EXEC sp_NhomHang_CRUD 'UPDATE', 'NMH001', N'Nhóm 1 mới';
EXEC sp_NhomHang_CRUD 'SELECT', 'NMH001', NULL;       -- Xem MH001

-- Ví dụ sử dụng:
EXEC sp_HangHoa_CRUD 'INSERT', NULL, N'Mặt hàng 1', N'NMH001', N'Đỏ', 'L', N'Chất liệu cotton', N'Cái', 100, 2000000, NULL; -- Tạo HH001
EXEC sp_HangHoa_CRUD 'INSERT', NULL, N'Mặt hàng 2', N'NMH002', N'Xanh', 'M', N'Chống nước', N'Hộp', 50, 1500000, NULL; -- Tạo HH002
EXEC sp_HangHoa_CRUD 'INSERT', NULL, N'Mặt hàng 3', N'NMH001', N'Đỏ', 'L', N'Chất liệu cotton', N'Cái', Null, Null, NULL; -- Tạo HH003
EXEC sp_HangHoa_CRUD 'INSERT', NULL, N'Mặt hàng 4', N'NMH003', N'Đỏ', 'L', N'Chất liệu cotton', N'Cái', Null, Null, NULL; -- Tạo HH004
EXEC sp_HangHoa_CRUD 'SELECT', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL; -- Xem tất cả
EXEC sp_HangHoa_CRUD 'DELETE', 'HH003', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL; -- Xóa HH002
EXEC sp_HangHoa_CRUD 'INSERT', NULL, N'Mặt hàng mới', 'NMH001', N'Trắng', 'S', N'Nhẹ', N'Chiếc', 200, 3000000, NULL; -- Tái sử dụng HH002
EXEC sp_HangHoa_CRUD 'UPDATE', 'HH001', N'Mặt hàng 1 mới', 'NMH002', N'Vàng', 'XL', N'Chống cháy', N'Bộ', 150, 2500000, NULL;
EXEC sp_HangHoa_CRUD 'UPDATE', 'HH005', N'Mặt hàng 1 mới', 'NMH002', N'Vàng', 'XL', N'Chống cháy', N'Bộ', 0, 50000, NULL;
EXEC sp_HangHoa_CRUD 'SELECT', 'HH001', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL; -- Xem HH001
EXEC sp_HangHoa_CRUD @Action = 'SELECT_ONE',
					 @ID_Hanghoa = 'HH001';

EXEC sp_NhanVien_CRUD 'INSERT', NULL, N'Nguyễn Văn A', '1990-01-01', N'Nam', N'Hà Nội', N'0901234567', N'ava@example.com';
EXEC sp_NhanVien_CRUD 'INSERT', NULL, N'Nguyễn Văn B', '1990-01-01', N'Nữ', N'Hà Nội', N'0901234567', N'bva@example.com';
EXEC sp_NhanVien_CRUD 'INSERT', NULL, N'Nguyễn Văn C', '1990-01-01', N'Nam', N'Hà Nội', N'0901234567', N'cva@example.com';
EXEC sp_NhanVien_CRUD 'SELECT';
EXEC sp_NhanVien_CRUD 'UPDATE', 'NV001', N'Nguyễn Văn B', '1990-01-01', N'Nam', N'TP.HCM', N'0909876543', N'nvb@example.com';
EXEC sp_NhanVien_CRUD 'DELETE', 'NV001';

EXEC sp_KhachHang_CRUD @Action = 'INSERT', 
                       @TenKhachHang = N'Nguyễn Văn A', 
                       @DiaChi = N'Hà Nội', 
                       @SoDienThoai = N'0901234567', 
                       @Email = N'nva@example.com';
EXEC sp_KhachHang_CRUD @Action = 'SELECT';
EXEC sp_KhachHang_CRUD @Action = 'SELECT', @ID_KhachHang = 'KH001';
EXEC sp_KhachHang_CRUD @Action = 'UPDATE', 
                       @ID_KhachHang = 'KH001', 
                       @TenKhachHang = N'Nguyễn Văn B', 
                       @DiaChi = N'TP.HCM', 
                       @SoDienThoai = N'0919876543', 
                       @Email = N'nvb@example.com';
EXEC sp_KhachHang_CRUD @Action = 'DELETE', @ID_KhachHang = 'KH001';

EXEC sp_Login_CRUD 'INSERT', NULL, 'NV001', 'user1', '123456', N'Admin';
EXEC sp_Login_CRUD 'INSERT', NULL, 'NV002', 'user2', '123456', N'Manager';
EXEC sp_Login_CRUD 'SELECT';
EXEC sp_Login_CRUD 'UPDATE', 'LG001', 'NV0002', 'user2', 'newpass456', N'Manager';
EXEC sp_Login_CRUD 'DELETE', 'LG001';

DECLARE @Result INT, @Role NVARCHAR(50), @ID_NhanVien VARCHAR(6);
EXEC sp_Login 
    @Username = 'user1',
    @Password = '123456',  -- Mật khẩu plain text sẽ được mã hóa trong SP
    @Result = @Result OUTPUT,
    @Role = @Role OUTPUT,
    @ID_NhanVien = @ID_NhanVien OUTPUT;

IF @Result = 1
    PRINT N'Đăng nhập thành công! Role: ' + @Role + N', ID Nhân viên: ' + @ID_NhanVien;
ELSE
    PRINT N'Đăng nhập thất bại!';


DECLARE @LogoutResult INT;
EXEC sp_Logout 
    @Username = 'user1',
    @Result = @LogoutResult OUTPUT;

IF @LogoutResult = 1
    PRINT N'Đăng xuất thành công!';
ELSE
    PRINT N'Đăng xuất thất bại!';

EXEC sp_HangHoa_CRUD 'SELECT', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL; -- Xem tất cả

EXEC sp_NhaCungCap_CRUD 'INSERT', NULL, N'Công ty ABC', N'Hà Nội', N'0901234567', N'abc@example.com';
EXEC sp_NhaCungCap_CRUD 'SELECT';
EXEC sp_NhaCungCap_CRUD 'DELETE', 'NCC001';

EXEC sp_HoaDonNhap_CRUD 'INSERT', NULL, '2023-10-15', 0, N'Ghi chú', 'NV002', 'NCC001';
-- EXEC sp_HoaDonNhap_CRUD 'INSERT_DETAIL', 'HDN001', NULL, NULL, NULL, NULL, 'HH003', 20, 50000;
EXEC sp_HoaDonNhap_CRUD 'SELECT';
EXEC sp_HoaDonNhap_CRUD @Action = 'SELECT_DETAIL', 
                        @ID_HoaDonNhap = 'HDN001';

EXEC sp_HoaDonNhap_CRUD @Action = 'DELETE', 
                        @ID_HoaDonNhap = 'HDN004';

EXEC sp_HoaDonNhap_CRUD @Action = 'INSERT_DETAIL', 
                        @ID_HoaDonNhap = 'HDN002', 
                        @ID_HangHoa = 'HH004', 
                        @SoLuong = 10, 
                        @DonGia = 500000;

EXEC sp_HoaDonNhap_CRUD @Action = 'UPDATE_DETAIL', 
                        @ID_HoaDonNhap = 'HDN001', 
                        @ID_HangHoa = 'HH001', 
                        @SoLuong = 5, 
                        @DonGia = 50000;

EXEC sp_HoaDonNhap_CRUD @Action = 'DELETE_DETAIL', 
                        @ID_HoaDonNhap = 'HDN001', 
                        @ID_HangHoa = 'HH003';


EXEC sp_HangHoa_CRUD 'SELECT', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL; -- Xem tất cả
EXEC sp_NhanVien_CRUD 'SELECT';
EXEC sp_KhachHang_CRUD @Action = 'SELECT';

EXEC sp_HoaDon_CRUD @Action = 'INSERT', 
                    @ID_NhanVien = 'NV001', 
                    @ID_KhachHang = 'KH001', 
                    @NgayBan = '2023-10-20', 
                    @TongTien = 0, 
                    @DaThuTien = 0;

EXEC sp_HoaDon_CRUD @Action = 'INSERT_DETAIL', 
                    @ID_HoaDonBan = 'HDB001', 
                    @ID_HangHoa = 'HH002', 
                    @SoLuong = 10, 
                    @QuiCach = N'Hộp', 
                    @GiaBan = 50000, 
                    @BaoHanh = N'12 tháng';

EXEC sp_HoaDon_CRUD @Action = 'UPDATE_DETAIL', 
                    @ID_HoaDonBan = 'HDB002', 
                    @ID_HangHoa = 'HH002', 
                    @SoLuong = 10, 
                    @QuiCach = N'Hộp', 
                    @GiaBan = 50000, 
                    @BaoHanh = N'24 tháng';

EXEC sp_HoaDon_CRUD @Action = 'UPDATE', 
                    @ID_HoaDonBan = 'HDB001', 
                    @DaThuTien = 1;

EXEC sp_HoaDon_CRUD @Action = 'UPDATE', 
                    @ID_HoaDonBan = 'HDB001', 
                    @DaThuTien = 0;

EXEC sp_HoaDon_CRUD @Action = 'DELETE_DETAIL', 
                    @ID_HoaDonBan = 'HDB001', 
                    @ID_HangHoa = 'HH001';

EXEC sp_HoaDon_CRUD @Action = 'DELETE_DETAIL', 
                    @ID_HoaDonBan = 'HDB003', 
                    @ID_HangHoa = 'HH001';

EXEC sp_HoaDon_CRUD @Action = 'DELETE', 
                    @ID_HoaDonBan = 'HDB001'

					
EXEC sp_HangHoa_CRUD 'SELECT', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL; -- Xem tất cả
EXEC sp_HoaDon_CRUD 'SELECT', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL; -- Xem tất cả
EXEC sp_HoaDon_CRUD 'SELECT_HOADON_ONLY'; -- Xem tất cả

SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tblHangban'