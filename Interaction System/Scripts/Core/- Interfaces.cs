namespace InteractionSystem
{
    /// <summary>
    /// Use prefix `Cri` for the classes that implements it.
    /// </summary>
    public interface IInteractionCriteria
    {
        /// <summary> 
        /// Called every frame while player is looking at this object and this object is not Focused. 
        /// </summary>
        /// <returns> true if this object can interact in this frame </returns>
        bool CanInteract(InteractionPlayer player);
    }

    /// <summary>
    /// Use prefix `Fid` for the classes that implements it.
    /// </summary>
    public interface IInteractionFeedback
    {
        /// <summary> 
        /// Called once as soon as <see cref="CanInteract(InteractionPlayer)"/> returns true. 
        /// This means the object is "In Focus"
        /// Provide feedback to the player that they can interact with this object. 
        /// </summary>
        void OnFocusGained(InteractionPlayer player);

        /// <summary> 
        /// Called once right before the interaction starts.
        /// Cleanup for the feedback (if required).
        /// </summary>
        void OnBeforeInteract(InteractionPlayer player);

        /// <summary> 
        /// Called once when this object Looses Focuse (i.e. Player looks away without interacting)
        /// Cleanup for the feedback.
        /// </summary>
        void OnFocusLost(InteractionPlayer player);
    }

    /// <summary>
    /// Use prefix `Irn` for the classes that implements it.
    /// </summary>
    public interface IInteractionProcessor
    {
        /// <summary> 
        /// Called once after the interaction starts 
        /// </summary>
        void OnInteractionStart(InteractionPlayer player);

        /// <summary> 
        /// Called once when interaction Ends 
        /// </summary>
        void OnInteractionEnd(InteractionPlayer player);
    }
}