namespace twentySix.EventBus.Internal;

internal static class MayBe
{
    public static TR With<TI, TR>(this TI input, Func<TI, TR> evaluator) where TI : class where TR : class 
        => input is null ? null : evaluator(input);
}
