USE [GuichetAutomatique]
GO
/****** Object:  StoredProcedure [dbo].[sp_Virement]    Script Date: 2023-08-17 15:08:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[sp_Virement]
    @IdCompteProv INT,
    @IdCompteDest INT,
    @IdUtilisateur INT,
    @MontantTransaction DECIMAL(10, 2)
AS
BEGIN
    BEGIN TRY
        -- Vérification si le compte est de type chèque et appartient à l'utilisateur courant.
        DECLARE @TypeCompteProv NVARCHAR(50)
        DECLARE @SoldeProv DECIMAL(10, 2)
        DECLARE @LimiteCreditProv DECIMAL(10, 2)

        SELECT @TypeCompteProv = TypeCompte, @SoldeProv = Solde, @LimiteCreditProv = ISNULL(LimiteCredit, 0) FROM Comptes
        WHERE IdCompte = @IdCompteProv AND IdUtilisateur = @IdUtilisateur;

        -- Calculate the credit available including line of credit
        DECLARE @CreditDispo DECIMAL(10, 2);
        SET @CreditDispo = @LimiteCreditProv - @SoldeProv;

        -- Calculate the maximum overdraft amount including line of credit
        DECLARE @MaxDecouvert DECIMAL(10, 2);
		SET @MaxDecouvert = @CreditDispo + @SoldeProv + @LimiteCreditProv;

        -- Calculate the overdraft amount based on transaction
        DECLARE @OverdraftAmount DECIMAL(10, 2);
        SET @OverdraftAmount = @MontantTransaction - @SoldeProv;

        -- Begin the transaction
        BEGIN TRANSACTION;

        -- Scenarios for different cases
        IF @TypeCompteProv = 'Chèque'
        BEGIN
            -- Scenario 1: Client has line of credit
            IF @LimiteCreditProv > 0
            BEGIN
                IF @MontantTransaction <= @MaxDecouvert
                BEGIN
                    -- Deduct overdraft from line of credit for source account
                    UPDATE Comptes
                    SET Solde = Solde - @MontantTransaction,
                        LimiteCredit = LimiteCredit - @OverdraftAmount
                    WHERE IdCompte = @IdCompteProv;

                    -- Insert transaction record for source account
                    INSERT INTO Transactions (TypeTransaction, IdCompte, MontantTransaction, DateTransaction)
                    VALUES ('Virement', @IdCompteProv, -@MontantTransaction, GETDATE());

                    -- Update destination account
                    UPDATE Comptes
                    SET Solde = Solde + @MontantTransaction
                    WHERE IdCompte = @IdCompteDest;

                    -- Insert transaction record for destination account
                    INSERT INTO Transactions (TypeTransaction, IdCompte, MontantTransaction, DateTransaction)
                    VALUES ('Virement', @IdCompteDest, @MontantTransaction, GETDATE());

                    -- Commit the transaction
                    COMMIT TRANSACTION;
                END
                ELSE
                BEGIN
                    -- Handle insufficient funds or exceeding line of credit
                    THROW 50002, 'Impossible de procéder à la transaction puisque le montant du virement excède la limite de crédit permise.', 1;
                END
            END
            -- Scenario 2: Client does not have a line of credit
            ELSE
            BEGIN
                IF @MontantTransaction <= @SoldeProv
                BEGIN
                    -- Deduct transaction amount from account balance
                    UPDATE Comptes
                    SET Solde = Solde - @MontantTransaction
                    WHERE IdCompte = @IdCompteProv;

                    -- Insert transaction record for source account
                    INSERT INTO Transactions (TypeTransaction, IdCompte, MontantTransaction, DateTransaction)
                    VALUES ('Virement', @IdCompteProv, -@MontantTransaction, GETDATE());

                    -- Update destination account
                    UPDATE Comptes
                    SET Solde = Solde + @MontantTransaction
                    WHERE IdCompte = @IdCompteDest;

                    -- Insert transaction record for destination account
                    INSERT INTO Transactions (TypeTransaction, IdCompte, MontantTransaction, DateTransaction)
                    VALUES ('Virement', @IdCompteDest, @MontantTransaction, GETDATE());

                    -- Enregistrer la transaction
                    COMMIT TRANSACTION;
                END
                ELSE
                BEGIN
                    -- Si fonds insuffisant.
                    THROW 50002, 'Impossible de procéder à la transaction puisque le montant du virement excède la limite de votre compte.', 1;
                END
            END
        END
        ELSE
        BEGIN
            -- Handle non-checking accounts
            THROW 50003, 'Le compte de provenance doit être un compte chèque.', 1;
        END
    END TRY
    BEGIN CATCH
        -- Rollback transaction in case of error
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        THROW;
    END CATCH;
END
