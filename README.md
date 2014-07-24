eventstore-bnosql-sample
========================

Sample use of eventstore including vagrantfile to get it running on mono

This solution is a sample and has only been run in Xamarin Studio. It was written to show some samples of how to read and write events to Greg Young's [Event Store](http://geteventstore.com)

Good Luck :)

## Running EventStore with projections enabled. 

    mono-sgen EventStore.SingleNode.exe --run-projections=all


## Example projection for this solution that emits events

    options({});

    // fromAll() 
    // fromStream('streamId') | fromStreams(['sream1', 'stream2']) | fromCategory('category')
       //NOTE: fromCategory requires $by_category standard projection to be enabled

    // .foreachStream() | .partitionBy(function(e) { return e.body.useId; })

    fromCategory('simpleAggregate').when({
        $init: function () {
            return { count1: 0, count2: 0, other: 0, last: null, curr: null}; // initial state
        },
      
        BookedInFact: function(s, e) {
            //  emit('streamId', 'eventType', {/* event body */} [, {/* optional event metadata */}]);
            //  linkTo('streamId', e [, {/* optional link-to-event metadata */}]);
            //  copyTo('streamId', e);
            s.count1 += 1;
            if (e.body.OccurredAt) {
                var curr = new Date(e.body.OccurredAt);
                
                if (s.last != null) {
                    s.diff = s.last.getTime() - curr.getTime();
                    if (-100 <= s.diff && s.diff <= 100) {
                        emit('alerts-BookingsThatOccurredAboutTheSameTime', 'BookedInFact', e.data);
                    }
                }
                
                s.curr = curr;
                s.last = s.curr;
            }
            return s;
        },
                        
        MyNameChangedFact: function(s, e) {
            s.count2 += 1;
            return s;
        }
        
    })
    // .filterBy(f)/transformBy(f)
    // .outputTo(name[, namePattern]) // defines a name for the output stream               
                        
    //NOTE: filterBy(), transformBy(), emit(), linkTo(), copyTo() require "emit enabled" (see checkbox below)
                            
                        
	
## Example of projection that 