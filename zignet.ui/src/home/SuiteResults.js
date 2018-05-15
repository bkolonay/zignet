import React, { Component } from 'react';
import SuiteResultChart from './SuiteResultChart'

class SuiteResults extends Component {

  _getGroupButton() {
    if (this.props.suiteResultsGrouped)
      return <a href="/" className="btn btn-outline-primary" role="button" title="Click to ungroup results">Ungroup</a>
    else
      return <a href="/?group=true" className="btn btn-primary" role="button" title="Click to group results by environment">Group</a>
  }

  render() {
    return (
  	  <div className="container">
        <div className="text-right">
          {this._getGroupButton()}
        </div>
  	  	<div className="row">
          {this.props.suiteResults.map((suiteResult) =>
            <SuiteResultChart key={suiteResult.SuiteIds[0]}
                              suiteResult={suiteResult}
                              suiteResultsGrouped={this.props.suiteResultsGrouped} />
          )}
        </div>
      </div>
    );
  }
}

export default SuiteResults;
