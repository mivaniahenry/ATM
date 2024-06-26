USE [GuichetAutomatique]
GO
/****** Object:  StoredProcedure [dbo].[sp_Retrait]    Script Date: 2023-08-17 15:07:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[sp_Retrait]
    @IdCompte INT,
    @MontantTransaction DECIMAL(10, 2)
AS
BEGIN
    BEGIN TRY
	    -- Vérification que le retrait s'effectue par multiple de 10.
        IF @MontantTransaction % 10 <> 0
        BEGIN
            THROW 50000, 'Le montant de retrait doit être un multiple de 10.', 1;
        END
        -- Début de la transaction
        BEGIN TRANSACTION;

        -- Mise à jour de la balance du compte affecté.
        UPDATE Comptes
        SET Solde = Solde - @MontantTransaction
        WHERE IdCompte = @IdCompte;

        -- Enregistrement de la transaction.
        INSERT INTO Transactions (TypeTransaction, IdCompte, MontantTransaction, DateTransaction)
        VALUES ('Retrait', @IdCompte, -@MontantTransaction, GETDATE());

        -- Validation de la transaction.
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        -- Si erreur, annulation de la transaction.
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        THROW;
    END CATCH;
END