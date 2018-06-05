import React, { Component } from 'react';
import TestResultRow from './TestResultRow'
import HomeLink from '../common/HomeLink'

class LatestTestResultsList extends Component {

  _getGroupButton() {
    var suiteId = this.props.suiteId;
    if (this.props.grouped)
      return <a href={"/" + suiteId} className="btn btn-outline-primary float-right" role="button" title="Click to ungroup results">Ungroup</a>
    else
      return <a href={"/" + suiteId + "?group=true"} className="btn btn-primary float-right" role="button" title="Click to group tests by environment">Group</a>
  }

  _getTypeHeader(grouped)
  {
    if (grouped)
      return <th scope="col">Type</th>;
  }

  render() {
    return (
      <div className="container">
        <h4><HomeLink grouped={this.props.grouped}/></h4>
        <div className="row">
          <div className="col-4"/>
          <div className="col-4">
            <h2 className="text-center">{this.props.suiteName}</h2>
          </div>
          <div className="col-4">
            {this._getGroupButton()}
          </div>
        </div>
        <table className="table">
          <thead>
            <tr>
              <th scope="col">Name</th>
              {this._getTypeHeader(this.props.grouped)}
              <th scope="col">Status</th>
              <th scope="col">History</th>
            </tr>
          </thead>
          <tbody>
            {this.props.testResults.map((testResult) =>
              <TestResultRow key={testResult.TestResultID}
                                testResult={testResult}
                                testResultsGrouped={this.props.grouped} />
            )}        
          </tbody>
        </table>
      </div>
    );
  }
}

export default LatestTestResultsList;
