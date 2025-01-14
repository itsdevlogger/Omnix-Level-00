namespace InteractionSystem
{
    /// <summary> Checks if the object is ready to interact. Use Prefix `Cri` for classes that implement this. </summary>
    public interface IInteractionCriteria
    {
        /// <summary> Called every frame while the player is looking at this object, and the object is not ready to interact </summary>
        /// <returns> true if this object is ready to interact, false otherwise </returns>
        bool CanInteract(InteractionPlayer player);
    }

    /// <summary> Provides feedback (e.g. press E to interact). Use Prefix `Fid` for classes that implement this. </summary>
    public interface IInteractionFeedback
    {
        /// <summary> Called once when the object is ready to interact. Provide feedback indicating that the player can interact with this object. </summary>
        void OnFocusGained(InteractionPlayer player);

        /// <summary> Called once, right before the interaction starts. Hide the feedback (if required). </summary>
        void OnBeforeInteract(InteractionPlayer player);

        /// <summary> Called once when player looks away. Hide the feedback (if required). </summary>
        void OnFocusLost(InteractionPlayer player);
    }

    /// <summary> Handles interaction logic. Use prefix `Irn` for the classes that implements this. </summary>
    public interface IInteractable
    {
        /// <summary> Called once when the interaction starts </summary>
        void OnInteractionStart(InteractionPlayer player);

        /// <summary> Called once when interaction ends </summary>
        void OnInteractionEnd(InteractionPlayer player);
    }
}

