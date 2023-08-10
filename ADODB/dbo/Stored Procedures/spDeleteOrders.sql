CREATE PROCEDURE [dbo].[spDeleteOrders]
	@Year INTEGER = NULL,
	@Month INTEGER = NULL,
	@Status nvarchar(20) = NULL,
	@Product INTEGER = NULL
AS
BEGIN
	SET NOCOUNT ON
	BEGIN TRANSACTION
		BEGIN TRY
			DELETE FROM Orders
			WHERE (@Year IS NULL OR YEAR(CreatedDate) = @Year)
			AND (@Month IS NULL OR Month(CreatedDate) = @Month)
			AND (@Status IS NULL OR status = @Status)
			AND (@Product IS NULL OR ProductId = @Product);
			COMMIT TRANSACTION;
		END TRY
		BEGIN CATCH
			ROLLBACK TRANSACTION;
			PRINT 'Error occurred during delete operation.';
		END CATCH
END
GO