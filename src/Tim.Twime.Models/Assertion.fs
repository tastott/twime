namespace Tim.Twime.Models

type AssertionResult =
    | Fail = 0
    | Pass = 1

type Assertion<'T when 'T : null and 'T : equality>(value : 'T, result: AssertionResult, failureReason : string) =
    member this.Value = value
    member this.Result = result
    member this.FailureReason = result
    new(value) = Assertion(value, (if value = null then AssertionResult.Fail else AssertionResult.Pass), null)
    new(failureReason : string) = Assertion(null, AssertionResult.Fail, failureReason)


