CREATE PROCEDURE spGetOrdersByFilter
    @Month INT = NULL,
    @Year INT = NULL,
    @Status NVARCHAR(50) = NULL,
    @ProductId INT = NULL
AS
BEGIN
    SELECT Id, Status, CreatedDate, UpdatedDate, ProductId
    FROM [Orders]
    WHERE
        (@Month IS NULL OR MONTH(CreatedDate) = @Month)
        AND (@Year IS NULL OR YEAR(CreatedDate) = @Year)
        AND (@Status IS NULL OR Status = @Status)
        AND (@ProductId IS NULL OR ProductId = @ProductId)
END