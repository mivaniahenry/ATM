CREATE PROCEDURE sp_Depot
    @IdCompte INT,
    @MontantTransaction DECIMAL(10, 2)
AS
BEGIN
     BEGIN TRY
        -- Début de la transaction
        BEGIN TRANSACTION;

        -- Mise à jour de la balance du compte affecté à la hausse.
        UPDATE Comptes
        SET Solde = Solde + @MontantTransaction
        WHERE IdCompte = @IdCompte;

        -- Enregistrement de la transaction.
        INSERT INTO Transactions (TypeTransaction, IdCompte, MontantTransaction, DateTransaction)
        VALUES ('Dépot', @IdCompte, @MontantTransaction, GETDATE());

        -- Validation de la transaction.
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        -- Si erreur, annulation de la transaction.
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        THROW;
    END CATCH;
END;
